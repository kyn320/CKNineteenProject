using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMove : MonoBehaviour
{
    [SerializeField]
    Vector3 spiritDefaultVector;
    [SerializeField]
    Vector3 spiritSetVector;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject playerModel;
    [SerializeField]
    Vector3 playerDirection;
    [SerializeField]
    float playerDistance;
    [SerializeField]
    float playerGravity;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = spiritDefaultVector;


        if (player == null)
        {
            player = GameObject.Find("Player");
            Debug.LogError("Spirit Find the PlayerObject");
            if (player == null)
            {
                Debug.LogError("Spirit has no PlayerObject");
            }
        }
        if (playerModel == null)
        {
            playerModel = GameObject.Find("Player Character");
            Debug.LogError($"Spirit Find the {playerModel.name}");
            if (playerModel == null)
            {
                Debug.LogError("Spirit has no playerModel");
            }
        }
        if (playerDistance == 0)
        {
            playerDistance = transform.localPosition.x;
        }

    }

    // Update is called once per frame
    void Update()
    {
        spiritSetVector = playerModel.transform.forward * spiritDefaultVector.x + playerModel.transform.right * spiritDefaultVector.z;
        spiritSetVector.y = spiritDefaultVector.y;

        //방향값 할당
        playerDirection = new Vector3(player.transform.position.x + spiritSetVector.x, player.transform.position.y + spiritSetVector.y, player.transform.position.z + spiritSetVector.z) - transform.position;


        //만약 플레이어와 스피릿의 사이가 지정한 값 보다 멀리 떨어질 경우 플레이어 가까이로 이동
        if (Vector3.Distance(player.transform.position, transform.position) > playerDistance)
        {
            transform.position += playerDirection.normalized * (playerGravity * (Vector3.Distance(player.transform.position, transform.position) - playerDistance));
        }

        


    }
}
