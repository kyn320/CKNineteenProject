using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    //������Ʈ �Ҵ�
    [Header("Objects")]
    [SerializeField]
    private GameObject cameraAnchor;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject playerModel;
    [SerializeField]
    private GameObject spirit;


    //���� ����
    [Header("SpiritSetting")]
    [SerializeField]
    private float spiritGravity;

    //�Ѿ� ���� (�켱 �ۼ��غ��� ���� ��ũ���ͺ� ������Ʈ�� ������� ������)
    [Header ("BulletSetting")]
    public int attackNum = 0;
    //���� �����ۺ� ��Ÿ��
    [SerializeField]
    private float[] attackCooldown = { 1, };
    //���� �����ۺ� ��Ÿ�� Ÿ�̸�
    private float[] attackCooldownTimer = { 0, };
    //����Ű�� ���� �� ���ư� ������Ʈ�� ����ϱ������ �ð�
    [SerializeField]
    private float[] bulletDelayTimes = { 0.5f, };
    //���� �����ƺ� ������ �����ϴ� �ð� (���� �ִϸ��̼� ���� ���� ���������� �Ͽ����� ���� �� �ĵ��� ������ ���� �������ִ°��� ���� �� �ؼ� ������)
    [SerializeField]
    float[] attackDelayTime = { 1, };
    //���ư��Ե� ������Ʈ ������
    [SerializeField]
    private GameObject[] bullets;
    //���ư��� ������Ʈ�� ���ư��� �ӵ�
    [SerializeField]
    private float[] bulletSpeed = { 1, };
    //������Ʈ�� ���ư��� ������ ���Ͱ� 
    [SerializeField]
    private Vector3[] bulletSetVector = { Vector3.zero, };


    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        setVariable();
    }
    


    void Update()
    {
        for(int i = 0; attackCooldown.Length > i; i++ )
        {
            if (attackCooldown[i] > attackCooldownTimer[i])
            {
                attackCooldownTimer[i] += Time.deltaTime;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (attackPossible())
            {
                attack();
            }
        }

        if(player.GetComponent<PlayerMoveController>().playerMoveType != 0)
        {
            playerModel.transform.forward = cameraAnchor.transform.forward * 0.001f;
        }
    }

    void setVariable()
    {
        if(cameraAnchor == null)
        {
            cameraAnchor = GameObject.Find("Camera Anchor");
            Debug.LogWarning($"{this.gameObject.name} find is cameraAnchor");

            if (cameraAnchor == null)
                Debug.LogError($"{this.gameObject.name} has not cameraAnchor");
        }
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");
            Debug.LogWarning($"{this.gameObject.name} find is mainCamera");

            if (mainCamera == null)
                Debug.LogError($"{this.gameObject.name} has not mainCamera");
        }
        if (player == null)
        {
            player = GameObject.Find("Player");
            Debug.LogWarning($"{this.gameObject.name} find is player");

            if (player == null)
                Debug.LogError($"{this.gameObject.name} has not player");
        }
        if (playerModel == null)
        {
            playerModel = GameObject.Find("Player Character");
            Debug.LogWarning($"{this.gameObject.name} find is playerModel");

            if (playerModel == null)
                Debug.LogError($"{this.gameObject.name} has not playerModel");
        }
        if (spirit == null)
        {
            spirit = GameObject.Find("Spirit");
            Debug.LogWarning($"{this.gameObject.name} find is Spirit");

            if (spirit == null)
                Debug.LogError($"{this.gameObject.name} has not Spirit");
        }

        for (int i = 0; bullets.Length < i; i++)
        {
            if (bullets[i] == null)
            {
                Debug.LogError($"{this.gameObject.name} has not bullet");
            }
        }

        anim = GetComponent<Animator>();
    }


    void attack()
    {
        anim.SetInteger("AttackType", attackNum + 1);
        anim.SetBool("Attacking", true);
        anim.SetTrigger("Attack");

        //spirit�� ���������� �������� ���ϵ��� ���� + spirit�� ��ǥ���� ����
        spirit.GetComponent<SpiritMoveController>().spiritMoveBool = false;
        spirit.GetComponent<SpiritMoveController>().spiritAttackVector = bulletSetVector[attackNum];
        //spirit.GetComponent<SpiritMoveController>().transform.position = bulletburstVector;

        //�����߿� ���⸦ �ٲܰ�� �ٸ� ������ ���� �� �������� ���⿡ �Ű������� �־������
        //��ٿ� ���Ҵ��� + �������� �ƴ��� + ���� ���� �پ��ִ°�
        
            attackCooldownTimer[attackNum] = 0;
            player.GetComponent<PlayerMoveController>().playerMoveType = 1;
            StartCoroutine(attackDontMoveTimer(attackDelayTime[attackNum]));
            StartCoroutine(bulletBurst(bullets[attackNum], attackNum, bulletDelayTimes[attackNum]));
        
    }

    bool attackPossible()
    {
        if (attackCooldown[attackNum] < attackCooldownTimer[attackNum] &&
        player.GetComponent<PlayerMoveController>().playerMoveType == 0 &&
        player.GetComponent<PlayerMoveController>().playerGroundFoot == true)
            return true;
        return false;
    }
    IEnumerator attackDontMoveTimer(float endTime)
    {
        yield return new WaitForSeconds(endTime);

        anim.SetBool("Attacking", false);
        player.GetComponent<PlayerMoveController>().playerMoveType = 0;
    }

    IEnumerator bulletBurst(GameObject bullet, int attackNum, float bulletBurstTime)
    {
        yield return new WaitForSeconds(bulletBurstTime);

        //������Ʈ�� ��µ� ���Ͱ� ����
        Vector3 bulletburstVector;
        bulletburstVector = playerModel.transform.forward * bulletSetVector[attackNum].x + playerModel.transform.right * bulletSetVector[attackNum].z;
        bulletburstVector.y = bulletSetVector[attackNum].y;
        bulletburstVector += transform.position;

        //������ ���� �̵��ϵ��� ����
        spirit.GetComponent<SpiritMoveController>().spiritMoveBool = true;

        //������Ʈ�� ��ǥ ���Ͱ� ���ϴ� ����
        RaycastHit bulletBurstRay;
        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out bulletBurstRay);
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 50000, Color.blue);

        //������Ʈ�� ��ǥ ���Ͱ� ����
        Vector3 bulletdirection;
        if (bulletBurstRay.point == Vector3.zero)
            bulletdirection = mainCamera.transform.position + mainCamera.transform.forward * 50000;
        else
            bulletdirection = (bulletburstVector - bulletBurstRay.point) * -1;


        //�Ѿ� ��� ��ġ
        GameObject goBullet = Instantiate(bullet, bulletburstVector, new Quaternion(0, 0, 0, 0));

        //�Ѿ� ���� ����
        goBullet.GetComponent<Rigidbody>().velocity = bulletdirection.normalized * bulletSpeed[attackNum];
    }
}
