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
    public bool spiritMoveBool = true;

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject playerModel;

    //�÷��̾�� ���ɰ��� ����Ʈ �Ÿ�
    [SerializeField]
    float playerDefauleDistance;

    //������ �÷��̾� ���󰡴� �ӵ�
    public float defaultPlayerGravity = 10;
    public float playerGravity = 10;


    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = spiritDefaultVector;
        rigid = GetComponent<Rigidbody>();
        spiritMoveBool = true;


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
            playerDefauleDistance = Vector3.Distance(Vector3.zero, spiritDefaultVector);
            SpriteMove(spiritDefaultVector);
        }
        else
        {
            playerDefauleDistance = Vector3.Distance(Vector3.zero, spiritAttackVector);
            SpriteAttackMove(spiritAttackVector);
        }
    }

    //������ ��ǥ(spiritEndVector)�� ���� ������
    void SpriteMove(Vector3 spiritEndVector)
    {
        //������ �÷��̾��� �𵨸��� ���� ���⿡ �ٲ�� ���Ͱ�
        Vector3 spiritSetVector = playerModel.transform.right * spiritEndVector.x + playerModel.transform.forward * spiritEndVector.z;
        spiritSetVector.y = spiritEndVector.y;

        //���� ���Ͱ� �����ϴ� ���Ⱚ
        Vector3 playerTowardDirection = player.transform.position + spiritSetVector
            - transform.position;

        //Spirit�� �̵��ӵ�
        playerGravity = defaultPlayerGravity + player.GetComponent<PlayerMoveController>().speed;

        //���� �÷��̾�� ������ �Ÿ��� ������ �� ���� �ָ� ������ ��� �÷��̾� �����̷� �̵� (�̵��� 0.01f��ŭ ���̸� ���� ������ ������Ʈ�� ��ǥ�� ������ ����ؼ� ������ ���� �������� ����)
        if (Vector3.Distance(player.transform.position, transform.position) > playerDefauleDistance &&
            playerDefauleDistance + 0.1f / playerGravity < Vector3.Distance(player.transform.position, transform.position))
        {
            rigid.velocity = Time.deltaTime * playerTowardDirection.normalized * (playerGravity * (Vector3.Distance(player.transform.position, transform.position) - playerDefauleDistance));
        }
    }

    //���ݽ� ������ ���� ��ŸƮ ����(spiritAttackVector)�� ���� ������
    void SpriteAttackMove(Vector3 spiritEndVector)
    {
        //������ �÷��̾��� �𵨸��� ���� ���⿡ �ٲ�� ���������� ���Ͱ�
        Vector3 spiritSetVector = playerModel.transform.right * spiritEndVector.x + playerModel.transform.forward * spiritEndVector.z;
        spiritSetVector.y = spiritEndVector.y;

        //���� ���Ͱ� �����ϴ� ���Ⱚ
        Vector3 playerTowardDirection = player.transform.position + spiritSetVector
            - transform.position;

        //Spirit�� �̵��ӵ��� ��Һ��� ����
        playerGravity = defaultPlayerGravity * 2 + player.GetComponent<PlayerMoveController>().speed;

        //���� �÷��̾�� ���Ǹ��� ���̰� ������ �� ���� �ָ� ������ ��� �÷��̾� �����̷� �̵� (�̵��� 0.1f��ŭ ������ ���� ������ ������Ʈ�� ��ǥ�� ������ ����ؼ� ������ ���� �������� ����)
        if (Vector3.Distance(player.transform.position + spiritSetVector, rigid.transform.position) > 0.1f)
        {
            rigid.velocity = Time.deltaTime * playerTowardDirection.normalized * (playerGravity * Vector3.Distance(player.transform.position + spiritEndVector, rigid.transform.position));
        }
        //else�� �ּ��� Ǯ�� transform���� �����ȴ�. (velocity�� ������ Spirit�� Ƣ�� ������ �����)
        else
        {

            //�ش� �ּ��� Ǯ�� ���������� �̵��Ѵ�.
            //rigid.velocity = Time.deltaTime * playerTowardDirection.normalized * (playerGravity * Vector3.Distance(player.transform.position + spiritEndVector, rigid.transform.position));

            //�ش� �ּ��� Ǯ�� ���������� �̵��Ѵ�.
            rigid.transform.position = player.transform.position + spiritSetVector;
            rigid.velocity = Vector3.zero;
            
        }
    }
}
