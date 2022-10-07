using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PlayerMoveState : PlayerStateBase
{
    private UnitStatus status;

    [SerializeField]
    private GameObject playerModel;

    [ReadOnly]
    [ShowInInspector]
    private float moveSpeed = 1f;
    [SerializeField]
    private float backMoveSpeedMutiplyer = 0.5f;
    [SerializeField]
    private float gravity = 1f;

    [SerializeField]
    private bool allowMove = true;

    [ReadOnly]
    [ShowInInspector]
    private Vector3 inputVector;
    [ReadOnly]
    [ShowInInspector]
    private Vector3 cameraForwardVector;

    private Animator animator;
    private CharacterController characterController;

    [SerializeField]
    private CameraMoveController cameraMoveController;

    public UnityEvent<float> updateMoveSpeedEvent;

    protected override void Awake()
    {
        base.Awake();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        status = controller.GetStatus();
        animator = controller.GetAnimator();
    }

    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        isStay = true;

        enterEvent?.Invoke();
    }

    public override void Update()
    {
        if (allowMove)
        {
            Move();
        }

        var moveVector = controller.GetMoveVector();

        moveVector.y = -gravity;

        controller.SetMoveVector(moveVector);
        characterController.Move(moveVector * Time.deltaTime);

        animator.SetFloat("MoveX", inputVector.x);
        animator.SetFloat("MoveZ", inputVector.z);

        updateMoveSpeedEvent?.Invoke(inputVector.magnitude);

        if (!characterController.isGrounded)
        {
            controller.ChangeState(PlayerStateType.Air);
        }
        else if (Mathf.Abs(inputVector.x) + Mathf.Abs(inputVector.z) < 0.1f)
        {
            controller.ChangeState(PlayerStateType.Idle);
        }

    }
    private void Move()
    {
        //움직일 방향
        Vector3 viewVector = transform.forward * Mathf.Abs(inputVector.z) + transform.right
            * (inputVector.z < 0f ? -inputVector.x : inputVector.x);

        if (Mathf.Abs(inputVector.x) > 0.5f || Mathf.Abs(inputVector.z) > 0.5f)
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
            moveSpeed = status.currentStatus.GetElement(StatusType.MoveSpeed).CalculateTotalAmount();

            if (inputVector.x != 0 || inputVector.z != 0)
            {
                playerModel.transform.forward = viewVector;
            }

            var viewNoramlVector = viewVector.normalized;
            var moveVector = controller.GetMoveVector();

            if (inputVector.z < 0f)
            {
                cameraMoveController.SetBackMoveCamera(true);

                moveVector.x = viewNoramlVector.x * -backMoveSpeedMutiplyer * moveSpeed;
                moveVector.z = viewNoramlVector.z * -backMoveSpeedMutiplyer * moveSpeed;
            }
            else
            {
                cameraMoveController.SetBackMoveCamera(false);

                moveVector.x = viewNoramlVector.x * moveSpeed;
                moveVector.z = viewNoramlVector.z * moveSpeed;
            }

            controller.SetMoveVector(moveVector);
        }
    }

    public void UpdateInputVector(Vector3 inputVector)
    {
        if (!isStay)
            return;
        this.inputVector = inputVector;
    }

    public void UpdateForwardView(Vector3 forwardView)
    {
        if (!isStay)
            return;

        this.cameraForwardVector = forwardView;

        cameraForwardVector.y = 0;
        transform.forward = cameraForwardVector;
    }

    public override void Exit()
    {
        isStay = false;
        updateMoveSpeedEvent?.Invoke(0f);
        exitEvent?.Invoke();
    }

}
