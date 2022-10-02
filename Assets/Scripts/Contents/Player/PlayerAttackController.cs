using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{

    private const float MaxAimDistance = 50000f;

    //������Ʈ �Ҵ�
    [SerializeField]
    private GameObject cameraAnchor;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject player;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckAttackPossible())
            {
                StartAttack();
            }
        }

        if (playerMoveController.playerMoveType != 0)
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
        && playerMoveController.playerMoveType == 0
        && playerMoveController.isGrounded)
        {
            return true;
        }

        return false;
    }

    void StartAttack()
    {
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
        projectileSpawnPoint = playerModel.transform.forward * weaponData.SpawnVector.x + playerModel.transform.right * weaponData.SpawnVector.z;
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

        //������Ʈ�� ��ǥ ���Ͱ� ���ϴ� ����
        RaycastHit bulletBurstRay;
        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out bulletBurstRay);
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * MaxAimDistance, Color.blue);

        //������Ʈ�� ��ǥ ���Ͱ� ����
        Vector3 bulletdirection;
        if (bulletBurstRay.point == Vector3.zero)
            bulletdirection = mainCamera.transform.position + mainCamera.transform.forward * MaxAimDistance;
        else
            bulletdirection = -(projectileSpawnPoint - bulletBurstRay.point);

        var projectileController = projectileObject.GetComponent<ProjectileController>();

        //�ɷ�ġ ����ؼ� ����� �־��ּ���.
        var attackPower = 1;
        //�ɷ�ġ ����ؼ� ����� �־��ּ���.
        var isCritical = false;

        projectileController.SetStatus(attackPower, isCritical);
        projectileController.Shot(handBone.position
            , bulletdirection.normalized
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

}
