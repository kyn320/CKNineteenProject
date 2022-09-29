using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMoveController : MonoBehaviour
{
     //������ ��ǥ ����
     [SerializeField]
    Vector3 spiritDefaultVector;

    //�÷��̾ ���ݽ� �������� �� ���Ͱ�
    public Vector3 spiritAttackVector;
    //������ ������ ���ΰ��� ����
    public bool spiritMoveBool;

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject playerModel;

    //�÷��̾�� ���ɰ��� ��ǥ�Ÿ�
    [SerializeField]
    float playerDistance;

    //������ �÷��̾� ���󰡴� �ӵ�
    public float defaultPlayerGravity = 10;
    public float playerGravity = 10;


    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = spiritDefaultVector;
        rigid = GetComponent<Rigidbody>();


        if (player == null)
        {
            player = GameObject.Find("Player");
            Debug.Log("Spirit Find the PlayerObject");
            if (player == null)
            {
                Debug.LogError("Spirit has no PlayerObject");
            }
        }
        if (playerModel == null)
        {
            playerModel = GameObject.Find("Player Character");
            Debug.Log($"Spirit Find the {playerModel.name}");
            if (playerModel == null)
            {
                Debug.LogError("Spirit has no playerModel");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spiritMoveBool)
        {
            playerGravity = defaultPlayerGravity;
            playerDistance = Vector3.Distance(Vector3.zero, spiritDefaultVector);
            SpriteMove(spiritDefaultVector);
        }
        else
        {
            playerDistance = Vector3.Distance(Vector3.zero, spiritAttackVector);
            SpriteMove(spiritAttackVector);
        }
    }

    void SpriteMove(Vector3 spiritEndVector)
    {
        //������ �÷��̾��� �𵨸��� ���� ���⿡ �ٲ�� ���Ͱ�
        Vector3 spiritSetVector = playerModel.transform.right * spiritEndVector.x + playerModel.transform.forward * spiritEndVector.z;
        spiritSetVector.y = spiritEndVector.y;

        //���� ���͸� �÷��̾� �����ǿ� ���߾���
        Vector3 playerTowardDirection = player.transform.position + spiritSetVector
            - transform.position;

        //���� ���� �������̶�� Spirit�� �̵��ӵ��� ���� ������
        if (spiritMoveBool)
            playerGravity = defaultPlayerGravity + player.GetComponent<PlayerMoveController>().speed;
        else
            if (defaultPlayerGravity * 5 >= playerGravity)
                playerGravity += playerGravity / 10;

        //���� �÷��̾�� ���Ǹ��� ���̰� ������ �� ���� �ָ� ������ ��� �÷��̾� �����̷� �̵� (�̵��� 0.01f��ŭ ���̸� ���� ������ ������Ʈ�� ��ǥ�� ������ ����ؼ� ������ ���� �������� ����)
        if (Vector3.Distance(player.transform.position, transform.position) > playerDistance && 
            playerDistance + 0.1f/playerGravity < Vector3.Distance(player.transform.position, transform.position))
        {
            Debug.Log(Vector3.Distance(player.transform.position, transform.position));
            rigid.velocity = playerTowardDirection.normalized * (playerGravity * (Vector3.Distance(player.transform.position, transform.position) - playerDistance));
        }
        else
        {
            //�ּ��� Ǯ�� transform���� �����ȴ�. (velocity�� ������ Spirit�� Ƣ�� ������ �����)
            /*
            if (spiritMoveBool)
            {
                playerGravity = defaultPlayerGravity;
                rigid.transform.position = player.transform.position + spiritSetVector;
                rigid.velocity = Vector3.zero;
            }
            else
            {
                rigid.transform.position = player.transform.position + spiritSetVector;
                rigid.velocity = Vector3.zero;
            }
            */
        }
    }
}
