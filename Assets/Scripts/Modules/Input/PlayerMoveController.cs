using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField]
    private StatusInfoData status;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject cameraAnchor;
    [SerializeField]
    private GameObject playerModel;
    [SerializeField]
    private float speed = 1f;


    public bool groundFoot = true;
    public Animator anim;

    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");

            if (cameraAnchor == null)
                Debug.LogError($"{this.gameObject.name} has no {mainCamera.name}");
            else
                Debug.Log($"{this.gameObject.name} is Find {mainCamera.name}");
        }

        if (cameraAnchor == null)
        {
            cameraAnchor = GameObject.Find("Camera Anchor");
            
            if (cameraAnchor == null)
                Debug.LogError($"{this.gameObject.name} has no {cameraAnchor.name}");
            else
                Debug.Log($"{this.gameObject.name} is Find {cameraAnchor.name}");
        }

        if (playerModel == null)
            Debug.LogError($"{this.gameObject.name} has no model");

        if (status.StausDic[StatusType.MoveSpeed].GetAmount() == 0)
        {
            status.StausDic[StatusType.MoveSpeed].SetAmount(10);
            Debug.LogError($"{this.gameObject.name} moveSpeed set is 0");
        }

        if (status.StausDic[StatusType.JumpPower].GetAmount() == 0)
        {
            status.StausDic[StatusType.JumpPower].SetAmount(10);
            Debug.LogError($"{this.gameObject.name} JumpPower set is 0");
        }

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

        if (groundFoot == false)
        {
            speed = status.StausDic[StatusType.MoveSpeed].GetAmount() * 0.001f;

            /*
            if(rigid.velocity.x + rigid.velocity.z > (moveVector.normalized * status.StausDic[StatusType.MoveSpeed].GetAmount() * 50 * Time.fixedDeltaTime).x + (moveVector.normalized * status.StausDic[StatusType.MoveSpeed].GetAmount() * 50 * Time.fixedDeltaTime).z)
            {
                speed = 0;
            }
            */

            Vector3 velocityVector = moveVector.normalized * speed * 50 * Time.fixedDeltaTime;
            velocityVector.y = 0f;
            rigid.velocity += velocityVector;
            if (inputVector.x != 0 || inputVector.z != 0)
                playerModel.transform.forward = moveVector;
        }
        else
        {
            speed = status.StausDic[StatusType.MoveSpeed].GetAmount();

            rigid.velocity = moveVector.normalized * speed * 50 * Time.fixedDeltaTime;
            if (inputVector.x != 0 || inputVector.z != 0)
                playerModel.transform.forward = moveVector;
        }
    }

    void Jump()
    {
        groundFoot = false;
        anim.SetBool("Jump", true);
        anim.SetBool("GroundFoot", false);
        rigid.velocity = new Vector3 (rigid.velocity.x, rigid.velocity.y + status.StausDic[StatusType.JumpPower].GetAmount(), rigid.velocity.z);
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
    
    public void groundFootTrue()
    {
        groundFoot = true;
        anim.SetBool("Jump", false);
        anim.SetBool("GroundFoot", true);
        Debug.Log($"{this.gameObject.name} groundFootTrue");
    }

    public void groundFootFalse()
    {
        groundFoot = false;
        anim.SetBool("GroundFoot", false);
        Debug.Log($"{this.gameObject.name} groundFootFalse 작동");
    }

}
