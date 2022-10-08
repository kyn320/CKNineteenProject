using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerAttackState : PlayerStateBase
{
    [SerializeField]
    private GameObject spiritObject;
    private SpiritMoveController spiritMoveController;

    [ReadOnly]
    [ShowInInspector]
    private bool isAttack = false;

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

    protected override void Awake()
    {
        base.Awake();
        spiritMoveController = spiritObject.GetComponent<SpiritMoveController>();
    }

    private void Start()
    {
        animator = controller.GetAnimator();
        EquipmentSystem.Instance.updateAttackOrderList.AddListener(UpdateEquipList);
    }

    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        controller.UpdateBattleState(PlayerBattleStateType.Battle);

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
    }

    public override void Update()
    {
        return;
    }

    public void StartAttack(Vector3 aimPoint)
    {
        if (!CheckAttackPossible())
            return;

        this.aimPoint = aimPoint;

        isAttack = true;

        var weaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());

        //TODO :: ���ݼӵ� ������� �ִϸ��̼� �ӵ�
        //�ɷ�ġ ����ؼ� ���⿡ �־��ּ���.
        var attackSpeed = 1f;

        //TODO :: ���� ���� Ÿ�� ���� INDEX ����.
        animator.SetInteger("AttackType", weaponData.AttackAnimationType);
        animator.SetTrigger("Attack");
        animator.speed = attackSpeed;

        //spirit�� ���������� �������� ���ϵ��� ���� + spirit�� ��ǥ���� ����
        spiritMoveController.isMoveable = false;
    }

    public void SpawnWeapon()
    {
        //���� ��ȯ �� �� ������ ����
        var weaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());

        //������Ʈ�� ��µ� ���Ͱ� ����
        projectileSpawnPoint = transform.forward * weaponData.SpawnVector.z + transform.right * weaponData.SpawnVector.x;
        projectileSpawnPoint.y = weaponData.SpawnVector.y;
        projectileSpawnPoint += transform.position;

        spiritMoveController.SetPosition(projectileSpawnPoint);

        //���� ��ȯ
        projectileObject = Instantiate(weaponData.WorldObject);
        projectileObject.transform.position = handBone.position;
    }

    public void Shot()
    {
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
    }

    public void EndAttack()
    {
        //���� ����
        //���� �޺��� ���� �Է��� ���� �� �ֽ��ϴ�.
        isAttack = false;

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

    public override void Exit()
    {
        EndAttack();
        exitEvent?.Invoke();
    }
}