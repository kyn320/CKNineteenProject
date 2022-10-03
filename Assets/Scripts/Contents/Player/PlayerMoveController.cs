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

    private bool allowMove = true;
    private Vector3 inputVector;
    public float jumpOppositeSpeed = 0.001f;
    public float moveSpeed = 1f;
    public bool isGrounded = true;
    //무기에 따라서 달라지는 플레이어 움직임 타입 (type이 0이라면 움직이는것이 가능합니다.)
    public int moveType = 0;

    private Animator animator;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = playerModel.GetComponent<Animator>();
    }

    void Update()
    {
        if (!allowMove)
            return;

        switch (moveType)
        {
            case 0:
                Move();
                break;
            default:
                rigid.velocity = Vector3.zero;
                break;
        }
    }

    public void ChangeMoveType(int moveType)
    {
        this.moveType = moveType;
    }

    public void UpdateInputVector(Vector3 inputVector)
    {
        this.inputVector = inputVector;
    }

    private void Move()
    {
        //보는 정면 방향
        Vector3 forwardVector = new Vector3(cameraAnchor.transform.forward.x, 0f, cameraAnchor.transform.forward.z);

        //보는 오른쪽 방향
        Vector3 rightVector = new Vector3(cameraAnchor.transform.right.x, 0f, cameraAnchor.transform.right.z);

        //움직일 방향
        Vector3 moveVector = forwardVector * inputVector.x + rightVector * inputVector.z;

        if (Mathf.Abs(inputVector.x) > 0f || Mathf.Abs(inputVector.z) > 0f)
        {
            animator.SetInteger("Move", 1);
        }
        else
        {
            animator.SetInteger("Move", 0);
        }

        //땅 밟았을 경우에만 이동 가능 / 안 밟았을 경우 움직이긴 하되 매우 미미하다.
        if (isGrounded)
        {
            moveSpeed = status.StausDic[StatusType.MoveSpeed].GetAmount();

            rigid.velocity = moveVector.normalized * moveSpeed * Time.fixedDeltaTime;

            if (inputVector.x != 0 || inputVector.z != 0)
                playerModel.transform.forward = moveVector;
        }
        else
        {
            moveSpeed = status.StausDic[StatusType.MoveSpeed].GetAmount() * jumpOppositeSpeed;

            Vector3 velocityVector = moveVector.normalized * moveSpeed * Time.fixedDeltaTime;

            velocityVector.y = 0f;

            rigid.velocity += velocityVector;

            if (inputVector.x != 0 || inputVector.z != 0)
                playerModel.transform.forward = moveVector;
        }
    }

    public void Jump()
    {
        if (!isGrounded || moveType != 0)
            return;

        isGrounded = false;
        animator.SetBool("Jump", true);
        animator.SetBool("IsGrounded", false);
        rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y + status.StausDic[StatusType.JumpPower].GetAmount(), rigid.velocity.z);
    }

    public void EnterGround(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
            animator.SetBool("IsGrounded", true);
        }
    }

    public void ExitGround(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }

}
