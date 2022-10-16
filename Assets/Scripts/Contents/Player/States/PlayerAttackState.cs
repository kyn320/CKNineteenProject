using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PlayerAttackState : PlayerStateBase
{
    private enum AttackStateType
    {
        Wait,
        Start,
        Spawn,
        Shot
    }

    [SerializeField]
    private GameObject spiritObject;
    [SerializeField]
    private SpiritPivot spiritPivot;
    private SpiritMoveController spiritMoveController;

    [ReadOnly]
    [ShowInInspector]
    private bool isAttack = false;

    [ReadOnly]
    [ShowInInspector]
    private AttackStateType attackStateType;

    [ReadOnly]
    [ShowInInspector]
    private int currentWeaponIndex = 0;
    [ReadOnly]
    [ShowInInspector]
    private List<ItemSlot> equipSlotDatas = new List<ItemSlot>();

    private GameObject projectileObject;
    private Vector3 projectileSpawnPoint;
    [ReadOnly]
    [ShowInInspector]
    private Vector3 projectileDirection;
    [ReadOnly]
    [ShowInInspector]
    private Vector3 aimPoint;
    [SerializeField]
    private Transform handBone;

    private Animator animator;

    [SerializeField]
    protected StatusCalculator damageCalculator;
    [SerializeField]
    protected StatusCalculator criticalDamageCalculator;

    [SerializeField]
    private GameObject sonicBoomVFX;

    public UnityEvent<int> updateWeaponIndexEvent;
    public UnityEvent<bool> updateIsAttackEvent;

    [SerializeField]
    private Vector3 forwardDirection;
    [SerializeField]
    private bool isMoveable = false;

    protected override void Awake()
    {
        base.Awake();
        spiritMoveController = spiritObject.GetComponent<SpiritMoveController>();
    }

    private void Start()
    {
        animator = controller.GetAnimator();
        EquipmentSystem.Instance.updateAttackOrderList.AddListener(UpdateEquipList);
        attackStateType = AttackStateType.Wait;
        updateWeaponIndexEvent?.Invoke(currentWeaponIndex);
    }

    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        controller.UpdateBattleState(PlayerBattleStateType.Battle);
        attackStateType = AttackStateType.Wait;
        enterEvent?.Invoke();
    }

    public bool CheckAttackPossible()
    {
        if (!isAttack
        && equipSlotDatas.Count > 0
        && (controller.GetState() == PlayerStateType.Idle || controller.GetState() == PlayerStateType.Move))
        {
            return true;
        }

        return false;
    }
    public void UpdateEquipList(List<ItemSlot> equipItems)
    {
        equipSlotDatas = equipItems;
        currentWeaponIndex = 0;
        updateWeaponIndexEvent?.Invoke(currentWeaponIndex);
    }

    public void UpdateDamage(DamageInfo damageInfo)
    {
        ForceStopAttack();
    }

    public override void Update()
    {
        return;
    }

    public void StartAttack(Vector3 aimPoint)
    {
        if (!CheckAttackPossible())
            return;

        transform.forward = forwardDirection;

        this.aimPoint = aimPoint;

        isAttack = true;

        controller.UpdateBattleState(PlayerBattleStateType.Battle);
        attackStateType = AttackStateType.Start;

        var weaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());


        isMoveable = weaponData.IsMoveable;

        if (!isMoveable)
            updateIsAttackEvent?.Invoke(isAttack);

        //TODO :: ���ݼӵ� ������� �ִϸ��̼� �ӵ�
        //�ɷ�ġ ����ؼ� ���⿡ �־��ּ���.
        var attackSpeed = 1f;

        //TODO :: ���� ���� Ÿ�� ���� INDEX ����.
        animator.SetInteger("AttackType", weaponData.AttackAnimationType);
        animator.SetTrigger("Attack");
        animator.speed = attackSpeed;
    }

    public void SpawnWeapon()
    {
        if (!isAttack)
            return;

        attackStateType = AttackStateType.Spawn;

        //���� ��ȯ �� �� ������ ����
        var weaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());

        spiritPivot.SetOffset(weaponData.SpawnVector);

        //���� ��ȯ
        projectileObject = Instantiate(weaponData.WorldObject);
        projectileObject.transform.position = handBone.position;
        projectileObject.GetComponent<MagicWeaponSpawner>().SetMovePoints(spiritPivot.transform, handBone, weaponData.SpawnTime);
    }

    public void Shot()
    {
        if (!isAttack)
            return;

        attackStateType = AttackStateType.Shot;
        //�� ������ ���� �� ���� ���� ����
        var weaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());

        //������ ���� �̵��ϵ��� ����
        spiritMoveController.isMoveable = true;

        //���� ���
        projectileDirection = aimPoint - handBone.transform.position;

        var projectileController = projectileObject.GetComponent<ProjectileController>();

        var isCritical = controller.GetStatus().GetCriticalSuccess();
        var damageAmount = 0f;

        if (isCritical)
        {
            damageAmount = damageCalculator.Calculate(controller.GetStatus().currentStatus);
        }
        else
        {
            damageAmount = criticalDamageCalculator.Calculate(controller.GetStatus().currentStatus);
        }

        spiritPivot.SetOriginOffset();
        sonicBoomVFX.SetActive(true);
        projectileController.hitEvnet.AddListener(SuccessHit);
        projectileController.SetStatus(damageAmount, isCritical);
        projectileController.Shot(handBone.position
            , projectileDirection.normalized
            , weaponData.StatusInfoData.GetElement(StatusType.ThrowSpeed).GetAmount()
            , weaponData.StatusInfoData.GetElement(StatusType.AttackDistance).GetAmount());

        //���� �ʱ�ȭ
        projectileObject = null;
        projectileSpawnPoint = Vector3.zero;
        currentWeaponIndex = (int)Mathf.Repeat(currentWeaponIndex + 1, equipSlotDatas.Count);
        updateWeaponIndexEvent?.Invoke(currentWeaponIndex);
    }

    public void EndAttack()
    {
        attackStateType = AttackStateType.Wait;
        //���� ����
        //���� �޺��� ���� �Է��� ���� �� �ֽ��ϴ�.
        isAttack = false;

        if (!isMoveable)
            updateIsAttackEvent?.Invoke(isAttack);

        isMoveable = false;
        //���� �ӵ��� �����մϴ�.
        animator.speed = 1f;

        animator.SetInteger("AttackType", 0);
    }

    public void SuccessHit(bool isKill)
    {
        ComboSystem.Instance.AddHitCombo(1);

        if (isKill)
            ComboSystem.Instance.AddKillCombo(1);
    }

    public void ForceStopAttack()
    {
        if (projectileObject != null)
        {
            Destroy(projectileObject);
        }

        spiritPivot.SetOriginOffset();
        projectileObject = null;
        projectileSpawnPoint = Vector3.zero;
        EndAttack();
    }

    public override void Exit()
    {
        EndAttack();
        exitEvent?.Invoke();
    }

    public void UpdateForwardView(Vector3 forwardView)
    {
        forwardView.y = 0;
        forwardDirection = forwardView;
    }
}