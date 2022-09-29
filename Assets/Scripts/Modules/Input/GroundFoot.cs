using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFoot : MonoBehaviour
{
    [SerializeField]
    private GameObject playerMoveController;

    private void Start()
    {
        if (playerMoveController == null)
        {
            playerMoveController = GameObject.Find("Player");
            Debug.Log($"{this.gameObject.name} is find PlayerMoveController");
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        Debug.Log($"{this.gameObject.name}충돌한 오브젝트 이름 : {collision.gameObject.name}");
        if (collision.gameObject.tag == "Ground")
        {
            playerMoveController.GetComponent<PlayerMoveController>().groundFootTrue();
            Debug.Log("groundFootTrue");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log($"{this.gameObject.name}떨어진 오브젝트 이름 : {collision.gameObject.name}");
        if (collision.gameObject.tag == "Ground")
        {
            playerMoveController.GetComponent<PlayerMoveController>().groundFootFalse();
            Debug.Log("groundFootFalse");
        }
    }

}
