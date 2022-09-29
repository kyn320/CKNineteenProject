using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraAnchor;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject playerModel;
    [SerializeField]
    private GameObject spirit;
    [SerializeField]
    private float spiritGravity;

    public int attackNum = 0;
    [SerializeField]
    private GameObject[] bullets;
    [SerializeField]
    private float[] bulletBurstTimes;
    [SerializeField]
    private float[] bulletSpeed;
    [SerializeField]
    private Vector3[] bulletSetVector;


    [SerializeField]
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        setVariable();
    }
    


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attack();
        }
    }

    void setVariable()
    {
        if(cameraAnchor == null)
        {
            cameraAnchor = GameObject.Find("Camera Anchor");
            Debug.Log($"{this.gameObject.name} find is cameraAnchor");

            if (cameraAnchor == null)
                Debug.LogError($"{this.gameObject.name} has not cameraAnchor");
        }
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");
            Debug.Log($"{this.gameObject.name} find is mainCamera");

            if (mainCamera == null)
                Debug.LogError($"{this.gameObject.name} has not mainCamera");
        }
        if (playerModel == null)
        {
            playerModel = GameObject.Find("Player Character");
            Debug.Log($"{this.gameObject.name} find is playerModel");

            if (playerModel == null)
                Debug.LogError($"{this.gameObject.name} has not playerModel");
        }
        if (spirit == null)
        {
            spirit = GameObject.Find("Spirit");
            Debug.Log($"{this.gameObject.name} find is Spirit");

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

        //spirit�� ���������� �������� ���ϵ��� ���� + spirit�� ��ǥ���� ����
        spirit.GetComponent<SpiritMoveController>().spiritMoveBool = false;
        spirit.GetComponent<SpiritMoveController>().spiritAttackVector = bulletSetVector[attackNum];
        //spirit.GetComponent<SpiritMoveController>().transform.position = bulletburstVector;

        //�����߿� ���⸦ �ٲܰ�� �ٸ� ������ ���� �� �������� ���⿡ �Ű������� �־������
        StartCoroutine(bulletBurst(bullets[attackNum], attackNum, bulletBurstTimes[attackNum]));
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
