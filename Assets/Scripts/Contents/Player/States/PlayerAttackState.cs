using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerAttackState : PlayerStateBase
{
    [SerializeField]
    private GameObject playerModel;

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

        //TODO :: 공격속도 기반으로 애니메이션 속도
        //능력치 계산해서 여기에 넣어주세요.
        var attackSpeed = 1f;

        //TODO :: 공격 무기 타입 별로 INDEX 대입.
        animator.SetInteger("AttackType", weaponData.AttackAnimationType);
        animator.SetTrigger("Attack");
        animator.speed = attackSpeed;

        //spirit가 개별적으로 움직이지 못하도록 설정 + spirit의 목표백터 변경
        spiritMoveController.isMoveable = false;
    }

    public void SpawnWeapon()
    {
        //무기 소환 및 선 딜레이 시작
        var weaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());

        //오브젝트가 출력될 벡터값 설정
        projectileSpawnPoint = playerModel.transform.forward * weaponData.SpawnVector.z + playerModel.transform.right * weaponData.SpawnVector.x;
        projectileSpawnPoint.y = weaponData.SpawnVector.y;
        projectileSpawnPoint += transform.position;

        spiritMoveController.SetPosition(projectileSpawnPoint);

        //무기 소환
        projectileObject = Instantiate(weaponData.WorldObject, handBone.position, Quaternion.identity);
    }

    public void Shot()
    {
        //선 딜레이 종료 및 공격 성공 판정
        var weaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());

        //정령이 자유 이동하도록 변경
        spiritMoveController.isMoveable = true;

        //방향 계산
        projectileDirection = aimPoint - handBone.transform.position;

        var projectileController = projectileObject.GetComponent<ProjectileController>();

        //능력치 계산해서 여기로 넣어주세요.
        var attackPower = 1;
        //능력치 계산해서 여기로 넣어주세요.
        var isCritical = false;

        projectileController.SetStatus(attackPower, isCritical);
        projectileController.Shot(handBone.position
            , projectileDirection.normalized
            , weaponData.StatusInfoData.GetElement(StatusType.ThrowSpeed).GetAmount()
            , weaponData.StatusInfoData.GetElement(StatusType.AttackDistance).GetAmount());

        //공격 초기화
        projectileObject = null;
        projectileSpawnPoint = Vector3.zero;
        currentWeaponIndex = (int)Mathf.Repeat(currentWeaponIndex + 1, equipSlotDatas.Count);
    }

    public void EndAttack()
    {
        //공격 종료
        //다음 콤보에 대한 입력을 받을 수 있습니다.
        isAttack = false;

        //원본 속도로 변경합니다.
        animator.speed = 1f;

        animator.SetInteger("AttackType", 0);
    }

    public override void Exit()
    {
        exitEvent?.Invoke();
    }
}