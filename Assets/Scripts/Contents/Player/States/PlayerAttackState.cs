using System;
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
        Attack,
        ComboCheck,
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
    private bool isCombo = false;

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
    private int currentComboIndex = 0;

    [ReadOnly]
    [ShowInInspector]
    private int currentHitCount = 0;

    [ReadOnly]
    [ShowInInspector]
    private List<ItemSlot> equipSlotDatas = new List<ItemSlot>();

    [ReadOnly]
    [ShowInInspector]
    private List<GameObject> weaponSpawnObjectList = new List<GameObject>();
    [ReadOnly]
    [ShowInInspector]
    private List<Transform> weaponHandBoneList = new List<Transform>();

    private Vector3 weaponSpawnPoint;
    [ReadOnly]
    [ShowInInspector]
    private Vector3 projectileDirection;
    [ReadOnly]
    [ShowInInspector]
    private Vector3 aimPoint;

    [SerializeField]
    private Transform[] handBones = new Transform[2];

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
        if (isCombo)
            return true;

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

        currentHitCount = 0;
        transform.forward = forwardDirection;

        this.aimPoint = aimPoint;

        isAttack = true;
        isCombo = false;

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
        animator.SetFloat("AttackCombo", currentComboIndex + 1);
        animator.SetTrigger("Attack");
        animator.speed = attackSpeed;

        switch (currentAttackWeaponData.AttackType)
        {
            case WeaponAttackType.None:
                break;
            case WeaponAttackType.Melee:
                controller.GetRigidbody().velocity = Vector3.zero;
                break;
            case WeaponAttackType.Projectile:
                break;
        }
    }

    public void SpawnWeapon()
    {
        if (!isAttack)
            return;

        attackStateType = AttackStateType.Spawn;

        GameObject weaponObject = null;

        //무기 소환 및 선 딜레이 시작
        //spiritPivot.SetOffset(handBone.position);

        switch (currentAttackWeaponData.HandType)
        {
            case HandType.Left:
                {
                    weaponObject = Instantiate(currentAttackWeaponData.WorldObject);
                    weaponObject.transform.SetParent(handBones[0]);
                    weaponObject.transform.localRotation = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].rotatation;
                    weaponObject.transform.localPosition = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].position;
                    weaponSpawnObjectList.Add(weaponObject);
                    weaponHandBoneList.Add(handBones[0]);

                    var vfxData = currentAttackWeaponData.AttackVFXDataList[currentComboIndex];
                    var spawnPrefab = vfxData.GetVFXPrefab("Spawn");
                    if (spawnPrefab != null)
                    {
                        var vfxObject = Instantiate(vfxData.GetVFXPrefab("Spawn"), handBones[0]);
                    }

                    var weaponAnimator = weaponObject.GetComponent<Animator>();
                    weaponAnimator?.SetTrigger("Spawn");
                }
                break;
            case HandType.Right:
                {
                    weaponObject = Instantiate(currentAttackWeaponData.WorldObject);
                    weaponObject.transform.SetParent(handBones[1]);
                    weaponObject.transform.localRotation = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].rotatation;
                    weaponObject.transform.localPosition = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].position;
                    weaponSpawnObjectList.Add(weaponObject);
                    weaponHandBoneList.Add(handBones[1]);
                    var vfxData = currentAttackWeaponData.AttackVFXDataList[currentComboIndex];
                    var spawnPrefab = vfxData.GetVFXPrefab("Spawn");
                    if (spawnPrefab != null)
                    {
                        var vfxObject = Instantiate(vfxData.GetVFXPrefab("Spawn"), handBones[1]);
                    }

                    var weaponAnimator = weaponObject.GetComponent<Animator>();
                    weaponAnimator?.SetTrigger("Spawn");
                }
                break;
            case HandType.All:
                {
                    for (var i = 0; i < 2; ++i)
                    {
                        if (i == 0)
                        {
                            weaponObject = Instantiate(currentAttackWeaponData.WorldObject);
                        }
                        else
                        {
                            weaponObject = Instantiate(currentAttackWeaponData.SubWeaponList[i - 1]);
                        }

                        weaponObject.transform.SetParent(handBones[i]);
                        weaponObject.transform.localRotation = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].rotatation;
                        weaponObject.transform.localPosition = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].position;
                        weaponSpawnObjectList.Add(weaponObject);
                        weaponHandBoneList.Add(handBones[i]);

                        var vfxData = currentAttackWeaponData.AttackVFXDataList[currentComboIndex];
                        var spawnPrefab = vfxData.GetVFXPrefab("Spawn");
                        if (spawnPrefab != null)
                        {
                            var vfxObject = Instantiate(vfxData.GetVFXPrefab("Spawn"), handBones[i]);
                        }

                        var weaponAnimator = weaponObject.GetComponent<Animator>();
                        weaponAnimator?.SetTrigger("Spawn");
                    }
                }
                break;
        }
    }

    public void CreateAttackVFX()
    {
        var vfxData = currentAttackWeaponData.AttackVFXDataList[currentComboIndex];
        var vfxObject = Instantiate(vfxData.GetVFXPrefab("Attack"), transform);
    }

    public void PlayAttack()
    {
        if (!isAttack)
            return;

        attackStateType = AttackStateType.Attack;

        //정령이 자유 이동하도록 변경
        spiritMoveController.isMoveable = true;

        var statusinfo = currentAttackWeaponData.StatusInfoData;

        //무기 능력치 적용
        controller.GetStatus().currentStatus.AddStatusInfo(currentAttackWeaponData.StatusInfoData);

        //피해량 선언
        var isCritical = false;
        var damageAmount = 0f;

        switch (currentAttackWeaponData.AttackType)
        {
            case WeaponAttackType.None:
                break;
            case WeaponAttackType.Melee:
                for (var i = 0; i < weaponSpawnObjectList.Count; ++i)
                {
                    var weaponController = weaponSpawnObjectList[i].GetComponent<WeaponController>();
                    weaponController.SetOwnerObject(this.gameObject);
                    weaponController.SetWeaponData(currentAttackWeaponData);
                    weaponController.hitEvnet.AddListener(SuccessHit);

                    //피해량을 계산합니다.
                    isCritical = CalculateCritical();
                    damageAmount = CalculateDamageAmount(isCritical);

                    weaponController.SetCalculator(CalculateCritical, CalculateDamageAmount);
                    weaponController.CreateAttackHitBox(currentComboIndex);
                }
                break;
            case WeaponAttackType.Projectile:
                for (var i = 0; i < weaponSpawnObjectList.Count; ++i)
                {
                    var weaponObject = weaponSpawnObjectList[i];
                    var handBone = weaponHandBoneList[i];

                    weaponObject.transform.SetParent(null);

                    //방향 계산     
                    projectileDirection = aimPoint - handBone.transform.position;

                    var vfxData = currentAttackWeaponData.AttackVFXDataList[currentComboIndex];
                    var vfxObject = Instantiate(vfxData.GetVFXPrefab("Shot"), transform);

                    var projectileController = weaponObject.GetComponent<ProjectileController>();
                    projectileController.SetStatus(controller.GetStatus().currentStatus);
                    projectileController.SetOwnerObject(this.gameObject);
                    projectileController.hitEvnet.AddListener(SuccessHit);

                    //피해량을 계산합니다.
                    isCritical = CalculateCritical();
                    damageAmount = CalculateDamageAmount(isCritical);

                    projectileController.SetCalculator(damageCalculator, criticalDamageCalculator);
                    projectileController.Shot(handBone.position
                        , aimPoint
                        , projectileDirection.normalized
                        , currentAttackWeaponData.StatusInfoData.GetElement(StatusType.ThrowSpeed).GetAmount()
                        , currentAttackWeaponData.StatusInfoData.GetElement(StatusType.AttackDistance).GetAmount());
                }
                break;
        }

        //무기 능력치 효과 해제

        if (currentComboIndex < currentAttackWeaponData.ComboCount - 1)
        {
            attackStateType = AttackStateType.ComboCheck;
            ++currentComboIndex;
        }
    }

    public void DissapearWeapon()
    {
        switch (currentAttackWeaponData.AttackType)
        {
            case WeaponAttackType.None:
                break;
            case WeaponAttackType.Melee:
                for (var i = 0; i < weaponSpawnObjectList.Count; ++i)
                {
                    var vfxData = currentAttackWeaponData.AttackVFXDataList[0];
                    var vfxObject = Instantiate(vfxData.GetVFXPrefab("Dissapear"), handBones[1]);

                    var weaponAnimator = weaponSpawnObjectList[0].GetComponent<Animator>();
                    weaponAnimator.SetTrigger("Dissapear");

                    vfxObject.transform.SetParent(null);
                    weaponSpawnObjectList[0].transform.SetParent(null);
                }
                break;
            case WeaponAttackType.Projectile:
                //TODO :: 여기 어떻게 할지 논의 중 // 아마 Spawn만 처리할 가능성 농후
                break;
        }
    }

    public void AllowCombo()
    {
        //다음 콤보에 대한 입력을 받을 수 있습니다.
        isCombo = attackStateType == AttackStateType.ComboCheck;
    }

    public void EndAttack()
    {
        if (!isAttack)
            return;

        controller.GetStatus().currentStatus.SubStatusInfo(currentAttackWeaponData.StatusInfoData);
        attackStateType = AttackStateType.Wait;

        isAttack = false;
        isCombo = false;

        if (!isMoveable)
            updateIsAttackEvent?.Invoke(isAttack);

        isMoveable = false;
        //원본 속도로 변경합니다.
        animator.speed = 1f;

        animator.SetInteger("AttackType", 0);
        DissapearWeapon();
        weaponSpawnObjectList.Clear();
        weaponHandBoneList.Clear();

        weaponSpawnPoint = Vector3.zero;

        //공격 초기화
        currentComboIndex = 0;
        Debug.Log("Update Weapon Index");
        currentWeaponIndex = (int)Mathf.Repeat(currentWeaponIndex + 1, equipSlotDatas.Count);
        updateWeaponIndexEvent?.Invoke(currentWeaponIndex);

        //공격 종료
    }

    public void SuccessHit(bool isKill)
    {
        ++currentHitCount;

        if (currentAttackWeaponData.AttackType == WeaponAttackType.Melee)
        {
            var hitBoxData = currentAttackWeaponData.HitBoxDataList[currentComboIndex];

            if (currentHitCount == 1)
            {
                CameraMoveController.Instance.PlayTweenAnimation(hitBoxData.HitTweeDataList);
            }

            if (currentHitCount == 3)
            {
                Instantiate(hitBoxData.ScreenVFXVolumeData.GetVFXPrefab("Hit"));
                GameTimeController.Instance.ChangeTimeScale(hitBoxData.TimeScale, hitBoxData.TimeScaleLifeTime);
            }
        }

        ComboSystem.Instance.AddHitCombo(1);

        if (isKill)
            ComboSystem.Instance.AddKillCombo(1);
    }

    private bool CalculateCritical()
    {
        return controller.GetStatus().GetCriticalSuccess();
    }

    private float CalculateDamageAmount(bool isCritical)
    {
        if (isCritical)
        {
            return criticalDamageCalculator.Calculate(controller.GetStatus().currentStatus);
        }
        else
        {
            return damageCalculator.Calculate(controller.GetStatus().currentStatus);
        }
    }

    public void ForceStopAttack()
    {
        if (!isAttack)
            return;

        if (weaponSpawnObjectList.Count > 0)
        {
            for (var i = 0; i < weaponSpawnObjectList.Count; ++i)
            {

                Destroy(weaponSpawnObjectList[i]);
            }
        }

        spiritPivot.SetOriginOffset();
        EndAttack();
    }

    public override void Exit()
    {
        //EndAttack();
        exitEvent?.Invoke();
    }

    public void UpdateForwardView(Vector3 forwardView)
    {
        forwardView.y = 0;
        forwardDirection = forwardView;
    }

    public void MoveStep(int value, float stepSpeed)
    {
        controller.GetRigidbody().velocity = transform.forward * stepSpeed;
    }

}