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
        Debug.Log($"{this.gameObject.name}�浹�� ������Ʈ �̸� : {collision.gameObject.name}");
        if (collision.gameObject.tag == "Ground")
        {
            playerMoveController.GetComponent<PlayerMoveController>().groundFootTrue();
            Debug.Log("groundFootTrue");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log($"{this.gameObject.name}������ ������Ʈ �̸� : {collision.gameObject.name}");
        if (collision.gameObject.tag == "Ground")
        {
            playerMoveController.GetComponent<PlayerMoveController>().groundFootFalse();
            Debug.Log("groundFootFalse");
        }
    }

}
