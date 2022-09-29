using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    //오브젝트 할당
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


    //정령 설정
    [Header("SpiritSetting")]
    [SerializeField]
    private float spiritGravity;

    //총알 설정
    [Header ("BulletSetting")]
    public int attackNum = 0;
    [SerializeField]
    private float[] attackCooldown = { 1, };
    [SerializeField]
    private float[] attackCooldownTimer = { 0, };
    [SerializeField]
    float[] attackAnimTime = { 1, };
    [SerializeField]
    private GameObject[] bullets;
    [SerializeField]
    private float[] bulletBurstTimes = { 1, };
    [SerializeField]
    private float[] bulletSpeed = { 1, };
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
        anim.SetTrigger("Attack");

        //spirit가 개별적으로 움직이지 못하도록 설정 + spirit의 목표백터 변경
        spirit.GetComponent<SpiritMoveController>().spiritMoveBool = false;
        spirit.GetComponent<SpiritMoveController>().spiritAttackVector = bulletSetVector[attackNum];
        //spirit.GetComponent<SpiritMoveController>().transform.position = bulletburstVector;

        //공격중에 무기를 바꿀경우 다른 공격이 나갈 수 있음으로 여기에 매개변수를 넣어줘야함
        //쿨다운 남았는지 + 공격중이 아닌지 + 발이 땅에 붙어있는가
        
            attackCooldownTimer[attackNum] = 0;
            player.GetComponent<PlayerMoveController>().playerMoveType = 1;
            StartCoroutine(attackDontMoveTimer(attackAnimTime[attackNum]));
            StartCoroutine(bulletBurst(bullets[attackNum], attackNum, bulletBurstTimes[attackNum]));
        
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
        
        player.GetComponent<PlayerMoveController>().playerMoveType = 0;
    }

    IEnumerator bulletBurst(GameObject bullet, int attackNum, float bulletBurstTime)
    {
        yield return new WaitForSeconds(bulletBurstTime);

        //오브젝트가 출력될 벡터값 설정
        Vector3 bulletburstVector;
        bulletburstVector = playerModel.transform.forward * bulletSetVector[attackNum].x + playerModel.transform.right * bulletSetVector[attackNum].z;
        bulletburstVector.y = bulletSetVector[attackNum].y;
        bulletburstVector += transform.position;

        //정령이 자유 이동하도록 변경
        spirit.GetComponent<SpiritMoveController>().spiritMoveBool = true;

        //오브젝트의 목표 백터값 구하는 레이
        RaycastHit bulletBurstRay;
        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out bulletBurstRay);
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 50000, Color.blue);

        //오브젝트의 목표 백터값 설정
        Vector3 bulletdirection;
        if (bulletBurstRay.point == Vector3.zero)
            bulletdirection = mainCamera.transform.position + mainCamera.transform.forward * 50000;
        else
            bulletdirection = (bulletburstVector - bulletBurstRay.point) * -1;


        //총알 출력 위치
        GameObject goBullet = Instantiate(bullet, bulletburstVector, new Quaternion(0, 0, 0, 0));

        //총알 물리 벡터
        goBullet.GetComponent<Rigidbody>().velocity = bulletdirection.normalized * bulletSpeed[attackNum];
    }
}
