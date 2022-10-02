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

    public float jumpOppositeSpeed = 0.001f;
    public float moveSpeed = 1f;
    public bool isGrounded = true;
    //���⿡ ���� �޶����� �÷��̾� ������ Ÿ�� (type�� 0�̶�� �����̴°��� �����մϴ�.)
    public int playerMoveType = 0;

    private Animator animator;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = playerModel.GetComponent<Animator>();
    }

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

        if (Input.GetKey(KeyCode.Space) && isGrounded == true && playerMoveType == 0)
        {
            Jump();
        }
    }

    private void PlayerMove()
    {
        //���� ���� ����
        Vector3 forwardVector = new Vector3(cameraAnchor.transform.forward.x, 0f, cameraAnchor.transform.forward.z);

        //���� ������ ����
        Vector3 rightVector = new Vector3(cameraAnchor.transform.right.x, 0f, cameraAnchor.transform.right.z);

        //�÷��̾� �Է°�
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

        //������ ����
        Vector3 moveVector = forwardVector * inputVector.x + rightVector * inputVector.z;

        if (Mathf.Abs(inputVector.x) > 0f || Mathf.Abs(inputVector.z) > 0f)
        {
            animator.SetInteger("Move", 1);
        }
        else
        {
            animator.SetInteger("Move", 0);
        }

        //�� ����� ��쿡�� �̵� ���� / �� ����� ��� �����̱� �ϵ� �ſ� �̹��ϴ�.
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

    void Jump()
    {
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
