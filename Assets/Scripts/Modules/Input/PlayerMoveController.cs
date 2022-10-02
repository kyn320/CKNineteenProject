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

    public float speed = 1f;
    public bool playerGroundFoot = true;
    //무기에 따라서 달라지는 플레이어 움직임 타입 (type이 0이라면 움직이는것이 가능합니다.)
    public int playerMoveType = 0;


    public Animator anim;

    [SerializeField]
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
                Debug.LogWarning($"{this.gameObject.name} is Find {mainCamera.name}");
        }

        if (cameraAnchor == null)
        {
            cameraAnchor = GameObject.Find("Camera Anchor");
            
            if (cameraAnchor == null)
                Debug.LogError($"{this.gameObject.name} has no {cameraAnchor.name}");
            else
                Debug.LogWarning($"{this.gameObject.name} is Find {cameraAnchor.name}");
        }

        if (playerModel == null)
            Debug.LogWarning($"{this.gameObject.name} has no model");

        if (status.StausDic[StatusType.MoveSpeed].GetAmount() == 0)
        {
            status.StausDic[StatusType.MoveSpeed].SetAmount(10);
            Debug.LogWarning($"{this.gameObject.name} moveSpeed set is 0");
        }

        if (status.StausDic[StatusType.JumpPower].GetAmount() == 0)
        {
            status.StausDic[StatusType.JumpPower].SetAmount(10);
            Debug.LogWarning($"{this.gameObject.name} JumpPower set is 0");
        }

        //anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerMoveType)
        {
            case 0:
                PlayerMove();
                break;
            default:
                rigid.velocity = Vector3.zero;
                break;
        }

        if(Input.GetKey(KeyCode.Space) && playerGroundFoot == true && playerMoveType == 0)
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
            anim.SetInteger("Move", 0);
        }
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            anim.SetInteger("Move", 1);
        }

        if (playerGroundFoot == false)
        {
            speed = status.StausDic[StatusType.MoveSpeed].GetAmount() * 0.001f;

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
        playerGroundFoot = false;
        anim.SetBool("Jump", true);
        anim.SetBool("GroundFoot", false);
        rigid.velocity = new Vector3 (rigid.velocity.x, rigid.velocity.y + status.StausDic[StatusType.JumpPower].GetAmount(), rigid.velocity.z);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerGroundFoot = true;
            anim.SetBool("Jump", false);
            anim.SetBool("GroundFoot", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerGroundFoot = false;
            anim.SetBool("GroundFoot", false);
        }
    }
    
    public void groundFootTrue()
    {
        playerGroundFoot = true;
        anim.SetBool("Jump", false);
        anim.SetBool("GroundFoot", true);
        Debug.Log($"{this.gameObject.name} groundFootTrue");
    }

    public void groundFootFalse()
    {
        playerGroundFoot = false;
        anim.SetBool("GroundFoot", false);
        Debug.Log($"{this.gameObject.name} groundFootFalse 작동");
    }

}
