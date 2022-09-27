using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject cameraAnchor;
    public GameObject playerModel;
    public float speed = 1;
    public float jumpPower = 1;
    public bool groundFoot = true;

    [SerializeField]
    Animator anim;

    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");
        }
        else if (cameraAnchor == null)
        {
            Debug.LogError("player has no mainCamera");
        }

        if (cameraAnchor == null)
        {
            cameraAnchor = GameObject.Find("Camera Anchor");
        }
        else if (cameraAnchor == null)
        {
            Debug.LogError("player has no cameraAnchor");
        }

        if (playerModel == null)
            Debug.LogError("player has no model");

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        if(Input.GetKey(KeyCode.Space) && groundFoot == true)
        {
            Jump();
        }
    }

    private void PlayerMove()
    {
        if (groundFoot == false)
            return;

        //보는 정면 방향
        Vector3 flontVector = new Vector3(cameraAnchor.transform.forward.x, 0f, cameraAnchor.transform.forward.z);

        //보는 오른쪽 방향
        Vector3 flontRight = new Vector3(cameraAnchor.transform.right.x, 0f, cameraAnchor.transform.right.z);

        //플레이어 입력값
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

        //움직일 방향
        Vector3 moveVector = flontVector * inputVector.x + flontRight * inputVector.z;



        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            anim.SetInteger("Move",0);
        } 
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            anim.SetInteger("Move", 1);
        }

        rigid.velocity = moveVector.normalized * speed * Time.fixedDeltaTime;
        if (inputVector.x != 0 || inputVector.z != 0)
        playerModel.transform.forward = moveVector;
    }

    void Jump()
    {
        groundFoot = false;
        anim.SetBool("Jump", true);
        anim.SetBool("GroundFoot", false);
        rigid.velocity = new Vector3 (rigid.velocity.x, rigid.velocity.y + jumpPower, rigid.velocity.z);
    }



    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            groundFoot = true;
            anim.SetBool("Jump", false);
            anim.SetBool("GroundFoot", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            groundFoot = false;
            anim.SetBool("GroundFoot", false);
        }
    }
}
