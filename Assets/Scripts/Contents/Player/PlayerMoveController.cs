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
    [SerializeField]
    private Vector3 viewNoramlVector;
    [SerializeField]
    private Vector3 moveVector;

    public float moveSpeed = 1f;
    public float backMoveSpeed = 0.5f;
    public float gravity = 9.81f;

    //무기에 따라서 달라지는 플레이어 움직임 타입 (type이 0이라면 움직이는것이 가능합니다.)
    public int moveType = 0;

    private Animator animator;
    private CharacterController characterController;

    private void Awake()
    {
        animator = playerModel.GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (allowMove)
        {
            switch (moveType)
            {
                case 0:
                    Move();
                    break;
                default:
                    break;
            }
        }

        if (!characterController.isGrounded)
            moveVector.y -= gravity * Time.deltaTime;

        characterController.Move(moveVector * Time.deltaTime);

        animator.SetFloat("MoveX", inputVector.x);
        animator.SetFloat("MoveZ", inputVector.z);

        if (moveVector.y <= 0)
        {
            animator.SetBool("IsGrounded", characterController.isGrounded);
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

    public void UpdateForwardView(Vector3 forwardView)
    {
        forwardView.y = 0;
        transform.forward = forwardView;
    }

    private void Move()
    {
        //보는 정면 방향
        Vector3 forwardVector = new Vector3(cameraAnchor.transform.forward.x, 0f, cameraAnchor.transform.forward.z);

        //보는 오른쪽 방향
        Vector3 rightVector = new Vector3(cameraAnchor.transform.right.x, 0f, cameraAnchor.transform.right.z);

        //움직일 방향
        Vector3 viewVector = forwardVector * Mathf.Abs(inputVector.z) + rightVector
            * (inputVector.z < 0f ? -inputVector.x : inputVector.x);

        if (Mathf.Abs(inputVector.x) > 0f || Mathf.Abs(inputVector.z) > 0f)
        {
            animator.SetInteger("MoveSpeed", 1);
        }
        else
        {
            animator.SetInteger("MoveSpeed", 0);
        }

        //땅 밟았을 경우에만 이동 가능 / 안 밟았을 경우 움직이긴 하되 매우 미미하다.
        if (characterController.isGrounded)
        {
            moveSpeed = status.StausDic[StatusType.MoveSpeed].GetAmount();

            if (inputVector.x != 0 || inputVector.z != 0)
            {
                playerModel.transform.forward = viewVector;
            }

            viewNoramlVector = viewVector.normalized;

            if (inputVector.z < 0f)
            {
                moveVector.x = viewNoramlVector.x * -backMoveSpeed;
                moveVector.z = viewNoramlVector.z * -backMoveSpeed;
            }
            else
            {
                moveVector.x = viewNoramlVector.x * moveSpeed;
                moveVector.z = viewNoramlVector.z * moveSpeed;
            }
        }
    }

    public void Jump()
    {
        if (!characterController.isGrounded || moveType != 0)
            return;

        animator.SetTrigger("Jump");
        animator.SetBool("IsGrounded", false);
        moveVector.y = status.StausDic[StatusType.JumpPower].GetAmount();
    }

    public bool GetIsGround()
    {
        return characterController.isGrounded;
    }



}
