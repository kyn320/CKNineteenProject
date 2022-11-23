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

        //TODO :: ���ݼӵ� ������� �ִϸ��̼� �ӵ�
        //�ɷ�ġ ����ؼ� ���⿡ �־��ּ���.
        var attackSpeed = 1f;

        //TODO :: ���� ���� Ÿ�� ���� INDEX ����.
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

        //���� ��ȯ �� �� ������ ����
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
                    var vfxObject = Instantiate(vfxData.GetVFXPrefab("Spawn"), handBones[0]);

                    var weaponAnimator = weaponObject.GetComponent<Animator>();
                    weaponAnimator.SetTrigger("Spawn");
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
                    var vfxObject = Instantiate(vfxData.GetVFXPrefab("Spawn"), handBones[1]);

                    var weaponAnimator = weaponObject.GetComponent<Animator>();
                    weaponAnimator.SetTrigger("Spawn");
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

                        //var vfxData = currentAttackWeaponData.AttackVFXDataList[currentComboIndex];
                        //var vfxObject = Instantiate(vfxData.GetVFXPrefab("Spawn"), handBones[i]);

                        //var weaponAnimator = weaponObject.GetComponent<Animator>();
                        //weaponAnimator.SetTrigger("Spawn");
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

        //������ ���� �̵��ϵ��� ����
        spiritMoveController.isMoveable = true;

        //���� �ɷ�ġ ����
        controller.GetStatus().currentStatus.AddStatusInfo(currentAttackWeaponData.StatusInfoData);

        //���ط� ����
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

                    //���ط��� ����մϴ�.
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

                    //���� ���     
                    projectileDirection = aimPoint - handBone.transform.position;

                    var vfxData = currentAttackWeaponData.AttackVFXDataList[currentComboIndex];
                    var vfxObject = Instantiate(vfxData.GetVFXPrefab("Shot"), transform);

                    var projectileController = weaponObject.GetComponent<ProjectileController>();
                    projectileController.hitEvnet.AddListener(SuccessHit);

                    //���ط��� ����մϴ�.
                    isCritical = CalculateCritical();
                    damageAmount = CalculateDamageAmount(isCritical);

                    projectileController.SetCalculator(CalculateCritical, CalculateDamageAmount);
                    projectileController.Shot(handBone.position
                        , aimPoint
                        , projectileDirection.normalized
                        , currentAttackWeaponData.StatusInfoData.GetElement(StatusType.ThrowSpeed).GetAmount()
                        , currentAttackWeaponData.StatusInfoData.GetElement(StatusType.AttackDistance).GetAmount());
                }
                break;
        }

        //���� �ɷ�ġ ȿ�� ����
        controller.GetStatus().currentStatus.SubStatusInfo(currentAttackWeaponData.StatusInfoData);

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
                //TODO :: ���� ��� ���� ���� �� // �Ƹ� Spawn�� ó���� ���ɼ� ����
                break;
        }
    }

    public void AllowCombo()
    {
        //���� �޺��� ���� �Է��� ���� �� �ֽ��ϴ�.
        isCombo = attackStateType == AttackStateType.ComboCheck;
    }

    public void EndAttack()
    {
        if (!isAttack)
            return;

        attackStateType = AttackStateType.Wait;

        isAttack = false;
        isCombo = false;

        if (!isMoveable)
            updateIsAttackEvent?.Invoke(isAttack);

        isMoveable = false;
        //���� �ӵ��� �����մϴ�.
        animator.speed = 1f;

        animator.SetInteger("AttackType", 0);
        DissapearWeapon();
        weaponSpawnObjectList.Clear();
        weaponHandBoneList.Clear();

        weaponSpawnPoint = Vector3.zero;

        //���� �ʱ�ȭ
        currentComboIndex = 0;
        Debug.Log("Update Weapon Index");
        currentWeaponIndex = (int)Mathf.Repeat(currentWeaponIndex + 1, equipSlotDatas.Count);
        updateWeaponIndexEvent?.Invoke(currentWeaponIndex);

        //���� ����
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