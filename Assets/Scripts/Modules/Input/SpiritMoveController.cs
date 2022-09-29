using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMoveController : MonoBehaviour
{
     //정령의 목표 벡터
     [SerializeField]
    Vector3 spiritDefaultVector;

    //플레이어가 공격시 움직여야 할 백터값
    public Vector3 spiritAttackVector;
    //정령이 움직일 것인가의 여부
    public bool spiritMoveBool = true;

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject playerModel;

    //플레이어와 정령간의 디폴트 거리
    [SerializeField]
    float playerDefauleDistance;

    //정령이 플레이어 따라가는 속도
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

    //정령이 목표(spiritEndVector)를 향해 움직임
    void SpriteMove(Vector3 spiritEndVector)
    {
        //정령이 플레이어의 모델링이 보는 방향에 바뀌는 백터값
        Vector3 spiritSetVector = playerModel.transform.right * spiritEndVector.x + playerModel.transform.forward * spiritEndVector.z;
        spiritSetVector.y = spiritEndVector.y;

        //정령 백터가 가야하는 방향값
        Vector3 playerTowardDirection = player.transform.position + spiritSetVector
            - transform.position;

        //Spirit의 이동속도
        playerGravity = defaultPlayerGravity + player.GetComponent<PlayerMoveController>().speed;

        //만약 플레이어와 정령의 거리가 지정한 값 보다 멀리 떨어질 경우 플레이어 가까이로 이동 (이동시 0.01f만큼 차이를 두지 않으면 오브젝트가 목표로 가고자 계속해서 움직여 덜덜 떨리듯이 보임)
        if (Vector3.Distance(player.transform.position, transform.position) > playerDefauleDistance &&
            playerDefauleDistance + 0.1f / playerGravity < Vector3.Distance(player.transform.position, transform.position))
        {
            rigid.velocity = Time.deltaTime * playerTowardDirection.normalized * (playerGravity * (Vector3.Distance(player.transform.position, transform.position) - playerDefauleDistance));
        }
    }

    //공격시 정령이 공격 스타트 지점(spiritAttackVector)을 향해 움직임
    void SpriteAttackMove(Vector3 spiritEndVector)
    {
        //정령이 플레이어의 모델링이 보는 방향에 바뀌는 로컬포지션 벡터값
        Vector3 spiritSetVector = playerModel.transform.right * spiritEndVector.x + playerModel.transform.forward * spiritEndVector.z;
        spiritSetVector.y = spiritEndVector.y;

        //정령 백터가 가야하는 방향값
        Vector3 playerTowardDirection = player.transform.position + spiritSetVector
            - transform.position;

        //Spirit의 이동속도가 평소보다 빠름
        playerGravity = defaultPlayerGravity * 2 + player.GetComponent<PlayerMoveController>().speed;

        //만약 플레이어와 스피릿의 사이가 지정한 값 보다 멀리 떨어질 경우 플레이어 가까이로 이동 (이동시 0.1f만큼 범위를 두지 않으면 오브젝트가 목표로 가고자 계속해서 움직여 덜덜 떨리듯이 보임)
        if (Vector3.Distance(player.transform.position + spiritSetVector, rigid.transform.position) > 0.1f)
        {
            rigid.velocity = Time.deltaTime * playerTowardDirection.normalized * (playerGravity * Vector3.Distance(player.transform.position + spiritEndVector, rigid.transform.position));
        }
        //else의 주석을 풀면 transform으로 고정된다. (velocity값 때문에 Spirit가 튀는 현상을 잡아줌)
        else
        {

            //해당 주석을 풀면 물리값으로 이동한다.
            //rigid.velocity = Time.deltaTime * playerTowardDirection.normalized * (playerGravity * Vector3.Distance(player.transform.position + spiritEndVector, rigid.transform.position));

            //해당 주석을 풀면 고정값으로 이동한다.
            rigid.transform.position = player.transform.position + spiritSetVector;
            rigid.velocity = Vector3.zero;
            
        }
    }
}
