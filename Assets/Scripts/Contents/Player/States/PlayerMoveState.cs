using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PlayerMoveState : PlayerStateBase
{
    private UnitStatus status;

    [ReadOnly]
    [ShowInInspector]
    private float moveSpeed = 1f;
    [SerializeField]
    private float backMoveSpeedMutiplyer = 0.5f;

    [SerializeField]
    private bool allowMove = true;

    [ReadOnly]
    [ShowInInspector]
    private Vector3 inputVector;
    [ReadOnly]
    [ShowInInspector]
    private Vector3 cameraForwardVector;

    private Animator animator;

    [SerializeField]
    private CameraMoveController cameraMoveController;

    private bool isAttack = false;

    protected override void Awake()
    {
        base.Awake();
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
        controller.updateMoveSpeedEvent?.Invoke(1f);

        enterEvent?.Invoke();
    }

    public override void Update()
    {
        if (!isStay)
            return;

        if (allowMove)
        {
            Move();
        }

        if (isAttack)
            return;


        animator.SetFloat("MoveX", inputVector.x);
        animator.SetFloat("MoveZ", inputVector.z);

        if (!controller.IsGround())
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

        if (isAttack)
        {
            viewVector = transform.forward + transform.right;
        }
        else
        {
            if (Mathf.Abs(inputVector.x) > 0.5f || Mathf.Abs(inputVector.z) > 0.5f)
            {
                animator.SetInteger("MoveSpeed", 1);
            }
            else
            {
                animator.SetInteger("MoveSpeed", 0);
            }
        }

        //땅 밟았을 경우에만 이동 가능 / 안 밟았을 경우 움직이긴 하되 매우 미미하다.
        if (controller.IsGround())
        {
            moveSpeed = status.currentStatus.GetElement(StatusType.MoveSpeed).CalculateTotalAmount();

            if (!isAttack && (inputVector.x != 0 || inputVector.z != 0))
            {
                transform.forward = viewVector;
            }

            var viewNoramlVector = viewVector.normalized;
            var moveDirection = viewNoramlVector;
            var velocity = Vector3.zero;

            var isSlope = controller.CheckSlope();
            if (isSlope)
            {
                moveDirection = controller.GetSlopeDirection(moveDirection);
            }

            if (!isAttack)
            {
                if (inputVector.z < 0f)
                {
                    cameraMoveController.SetBackMoveCamera(true);
                    velocity = moveDirection * -backMoveSpeedMutiplyer * moveSpeed;
                }
                else
                {
                    cameraMoveController.SetBackMoveCamera(false);
                    velocity = moveDirection * moveSpeed;
                }
            }

            controller.GetRigidbody().velocity = velocity;
            controller.GetRigidbody().useGravity = !isSlope;
        }
    }

    public void UpdateInputVector(Vector3 inputVector)
    {
        if (!isStay || isAttack)
            return;
        this.inputVector = inputVector;
    }

    public void UpdateForwardView(Vector3 forwardView)
    {
        if (!isStay || isAttack)
            return;

        this.cameraForwardVector = forwardView;

        cameraForwardVector.y = 0;
        transform.forward = cameraForwardVector;
    }

    public void SetIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }

    public override void Exit()
    {
        isStay = false;
        controller.updateMoveSpeedEvent?.Invoke(0f);
        exitEvent?.Invoke();
    }

}
