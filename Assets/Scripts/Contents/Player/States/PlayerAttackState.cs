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
                weaponObject = Instantiate(currentAttackWeaponData.WorldObject);
                weaponObject.transform.SetParent(handBones[0]);
                weaponObject.transform.localRotation = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].rotatation;
                weaponObject.transform.localPosition = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].position;
                weaponSpawnObjectList.Add(weaponObject);
                weaponHandBoneList.Add(handBones[0]);
                break;
            case HandType.Right:
                weaponObject = Instantiate(currentAttackWeaponData.WorldObject);
                weaponObject.transform.SetParent(handBones[1]);
                weaponObject.transform.localRotation = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].rotatation;
                weaponObject.transform.localPosition = currentAttackWeaponData.PivotOffsetDataList[currentComboIndex].position;
                weaponSpawnObjectList.Add(weaponObject);
                weaponHandBoneList.Add(handBones[1]);
                break;
            case HandType.All:
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
                }
                break;
        }

        //���� ��ȯ

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

        //���ط� ���
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
                for (var i = 0; i < weaponSpawnObjectList.Count; ++i)
                {
                    var weaponController = weaponSpawnObjectList[i].GetComponent<WeaponController>();
                    weaponController.SetOwnerObject(this.gameObject);
                    weaponController.SetWeaponData(currentAttackWeaponData);
                    weaponController.hitEvnet.AddListener(SuccessHit);
                    weaponController.SetStatus(damageAmount, isCritical);
                    weaponController.CreateAttackHitBox(currentComboIndex);
                }
                break;
            case WeaponAttackType.Projectile:
                for (var i = 0; i < weaponSpawnObjectList.Count; ++i)
                {
                    var weaponObject = weaponSpawnObjectList[i];
                    var handBone = weaponHandBoneList[i];

                    weaponObject.transform.SetParent(null);
                    //spiritPivot.SetOriginOffset();
                    //sonicBoomVFX.SetActive(true);
                    //���� ���     
                    projectileDirection = aimPoint - handBone.transform.position;
                    Debug.Log(projectileDirection);
                    var projectileController = weaponObject.GetComponent<ProjectileController>();
                    projectileController.hitEvnet.AddListener(SuccessHit);
                    projectileController.SetStatus(damageAmount, isCritical);
                    projectileController.Shot(handBone.position
                        , aimPoint
                        , projectileDirection.normalized
                        , currentAttackWeaponData.StatusInfoData.GetElement(StatusType.ThrowSpeed).GetAmount()
                        , currentAttackWeaponData.StatusInfoData.GetElement(StatusType.AttackDistance).GetAmount());
                }
                break;
        }

        if (currentComboIndex < currentAttackWeaponData.ComboCount - 1)
        {
            attackStateType = AttackStateType.ComboCheck;
            ++currentComboIndex;
        }
    }

    public void AllowCombo()
    {
        //���� �޺��� ���� �Է��� ���� �� �ֽ��ϴ�.
        isAttack = attackStateType != AttackStateType.ComboCheck;
    }

    public void EndAttack()
    {
        attackStateType = AttackStateType.Wait;

        isAttack = false;

        if (!isMoveable)
            updateIsAttackEvent?.Invoke(isAttack);

        isMoveable = false;
        //���� �ӵ��� �����մϴ�.
        animator.speed = 1f;

        animator.SetInteger("AttackType", 0);

        switch (currentAttackWeaponData.AttackType)
        {
            case WeaponAttackType.None:
                break;
            case WeaponAttackType.Melee:
                if (weaponSpawnObjectList.Count > 0)
                {
                    Destroy(weaponSpawnObjectList[0]);
                }
                break;
            case WeaponAttackType.Projectile:
                break;
        }

        weaponSpawnObjectList.Clear();
        weaponHandBoneList.Clear();

        weaponSpawnPoint = Vector3.zero;

        //���� �ʱ�ȭ
        currentComboIndex = 0;
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
        EndAttack();
        exitEvent?.Invoke();
    }

    public void UpdateForwardView(Vector3 forwardView)
    {
        forwardView.y = 0;
        forwardDirection = forwardView;
    }
}