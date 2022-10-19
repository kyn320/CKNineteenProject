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
        Attack
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
    private WeaponData currentAttackWeaponData;

    [ReadOnly]
    [ShowInInspector]
    private int currentWeaponIndex = 0;
    [ReadOnly]
    [ShowInInspector]
    private List<ItemSlot> equipSlotDatas = new List<ItemSlot>();

    private GameObject weaponObject;
    private Vector3 weaponSpawnPoint;
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
    [ShowInInspector]
    [ReadOnly]
    private DamageInfo damageInfo;

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

        currentAttackWeaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());


        isMoveable = currentAttackWeaponData.IsMoveable;

        if (!isMoveable)
            updateIsAttackEvent?.Invoke(isAttack);

        //TODO :: 공격속도 기반으로 애니메이션 속도
        //능력치 계산해서 여기에 넣어주세요.
        var attackSpeed = 1f;

        //TODO :: 공격 무기 타입 별로 INDEX 대입.
        animator.SetInteger("AttackType", currentAttackWeaponData.AttackAnimationType);
        animator.SetTrigger("Attack");
        animator.speed = attackSpeed;
    }

    public void SpawnWeapon()
    {
        if (!isAttack)
            return;

        attackStateType = AttackStateType.Spawn;

        //무기 소환 및 선 딜레이 시작
        //spiritPivot.SetOffset(handBone.position);

        //무기 소환
        weaponObject = Instantiate(currentAttackWeaponData.WorldObject);
        weaponObject.transform.SetParent(handBone);
        weaponObject.transform.localPosition = Vector3.zero;
    }

    public void Shot()
    {
        if (!isAttack)
            return;

        attackStateType = AttackStateType.Attack;

        //정령이 자유 이동하도록 변경
        spiritMoveController.isMoveable = true;

        //피해량 계산
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

        switch (currentAttackWeaponData.AttackType)
        {
            case WeaponAttackType.None:
                break;
            case WeaponAttackType.Melee:
                var weaponController = weaponObject.GetComponent<WeaponController>();
                weaponController.SetOwnerObject(this.gameObject);
                weaponController.SetWeaponData(currentAttackWeaponData);
                weaponController.hitEvnet.AddListener(SuccessHit);
                weaponController.SetStatus(damageAmount, isCritical);
                weaponController.CreateAttackHitBox(0);
                break;
            case WeaponAttackType.Projectile:

                weaponObject.transform.SetParent(null);
                //spiritPivot.SetOriginOffset();
                sonicBoomVFX.SetActive(true);
                //방향 계산     
                projectileDirection = aimPoint - handBone.transform.position;
                var projectileController = weaponObject.GetComponent<ProjectileController>();
                projectileController.hitEvnet.AddListener(SuccessHit);
                projectileController.SetStatus(damageAmount, isCritical);
                projectileController.Shot(handBone.position
                    , aimPoint
                    , projectileDirection.normalized
                    , currentAttackWeaponData.StatusInfoData.GetElement(StatusType.ThrowSpeed).GetAmount()
                    , currentAttackWeaponData.StatusInfoData.GetElement(StatusType.AttackDistance).GetAmount());
                break;
        }

        //공격 초기화
        currentWeaponIndex = (int)Mathf.Repeat(currentWeaponIndex + 1, equipSlotDatas.Count);
        updateWeaponIndexEvent?.Invoke(currentWeaponIndex);
    }

    public void EndAttack()
    {
        attackStateType = AttackStateType.Wait;
        //공격 종료
        //다음 콤보에 대한 입력을 받을 수 있습니다.
        isAttack = false;

        if (!isMoveable)
            updateIsAttackEvent?.Invoke(isAttack);

        isMoveable = false;
        //원본 속도로 변경합니다.
        animator.speed = 1f;

        animator.SetInteger("AttackType", 0);

        switch (currentAttackWeaponData.AttackType)
        {
            case WeaponAttackType.None:
                break;
            case WeaponAttackType.Melee:
                if (weaponObject != null)
                {
                    Destroy(weaponObject);
                }
                break;
            case WeaponAttackType.Projectile:
                break;
        }

        weaponObject = null;
        weaponSpawnPoint = Vector3.zero;
    }

    public void SuccessHit(bool isKill)
    {
        ComboSystem.Instance.AddHitCombo(1);

        if (isKill)
            ComboSystem.Instance.AddKillCombo(1);
    }

    public void ForceStopAttack()
    {
        if (weaponObject != null)
        {
            Destroy(weaponObject);
        }

        spiritPivot.SetOriginOffset();
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