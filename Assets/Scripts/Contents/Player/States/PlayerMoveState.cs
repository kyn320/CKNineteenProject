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
        if (allowMove)
        {
            Move();
        }

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
        //������ ����
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

        //�� ����� ��쿡�� �̵� ���� / �� ����� ��� �����̱� �ϵ� �ſ� �̹��ϴ�.
        if (controller.IsGround())
        {
            moveSpeed = status.currentStatus.GetElement(StatusType.MoveSpeed).CalculateTotalAmount();

            if (inputVector.x != 0 || inputVector.z != 0)
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

            if (inputVector.z < 0f)
            {
                cameraMoveController.SetBackMoveCamera(true);
                velocity = moveDirection * - backMoveSpeedMutiplyer * moveSpeed;
            }
            else
            {
                cameraMoveController.SetBackMoveCamera(false);
                velocity = moveDirection * moveSpeed;
            }

            controller.GetRigidbody().velocity = velocity;
            controller.GetRigidbody().useGravity = !isSlope;
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
        controller.updateMoveSpeedEvent?.Invoke(0f);
        exitEvent?.Invoke();
    }

}
