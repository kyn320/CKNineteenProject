using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{


    //������Ʈ �Ҵ�
    [SerializeField]
    private GameObject cameraAnchor;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject playerModel;
    [SerializeField]
    private GameObject spiritObject;

    public bool isAttack = false;

    public int currentWeaponIndex = 0;
    public List<ItemSlot> equipSlotDatas;

    private Animator animator;

    private PlayerMoveController playerMoveController;
    private SpiritMoveController spiritMoveController;

    private GameObject projectileObject;
    private Vector3 projectileSpawnPoint;
    [SerializeField]
    private Vector3 projectileDirection;
    [SerializeField]
    private Vector3 aimPoint;
    [SerializeField]
    private Transform handBone;

    private void Awake()
    {
        animator = playerModel.GetComponent<Animator>();
        playerMoveController = gameObject.GetComponent<PlayerMoveController>();
        spiritMoveController = spiritObject.GetComponent<SpiritMoveController>();
    }

    private void Start()
    {
        EquipmentSystem.Instance.updateAttackOrderList.AddListener(UpdateEquipList);
    }

    void Update()
    {
        if (playerMoveController.moveType != 0)
        {
            playerModel.transform.forward = cameraAnchor.transform.forward * 0.001f;
        }
    }

    public void UpdateEquipList(List<ItemSlot> equipItems)
    {
        equipSlotDatas = equipItems;
    }

    bool CheckAttackPossible()
    {
        if (!isAttack
        && equipSlotDatas.Count > 0
        && playerMoveController.moveType == 0
        && playerMoveController.isGrounded)
        {
            return true;
        }

        return false;
    }

    public void StartAttack(Vector3 aimPoint)
    {
        if (!CheckAttackPossible())
            return;

        this.aimPoint = aimPoint;

        isAttack = true;

        playerMoveController.ChangeMoveType(1);

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
        projectileSpawnPoint = playerModel.transform.forward * weaponData.SpawnVector.z + playerModel.transform.right * weaponData.SpawnVector.x;
        projectileSpawnPoint.y = weaponData.SpawnVector.y;
        projectileSpawnPoint += transform.position;

        spiritMoveController.SetPosition(projectileSpawnPoint);

        //���� ��ȯ
        projectileObject = Instantiate(weaponData.WorldObject, handBone.position, Quaternion.identity);
    }

    public void Shot()
    {
        //�� ������ ���� �� ���� ���� ����
        var weaponData = (WeaponData)(equipSlotDatas[currentWeaponIndex].GetItemData());

        //������ ���� �̵��ϵ��� ����
        spiritMoveController.isMoveable = true;

        //���� ���
        projectileDirection = aimPoint -  handBone.transform.position;

        var projectileController = projectileObject.GetComponent<ProjectileController>();

        //�ɷ�ġ ����ؼ� ����� �־��ּ���.
        var attackPower = 1;
        //�ɷ�ġ ����ؼ� ����� �־��ּ���.
        var isCritical = false;

        projectileController.SetStatus(attackPower, isCritical);
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

        playerMoveController.ChangeMoveType(0);
    }

}
