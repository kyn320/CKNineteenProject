using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerStateBase
{
    [SerializeField]
    private float gravityScale = 2f;

    private Animator animator;
    private Vector3 inputVector;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        animator = controller.GetAnimator();
    }

    public override void Enter()
    {
        controller.GetRigidbody().useGravity = false;
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        isStay = true;

        enterEvent?.Invoke();
    }

    public override void Update()
    {
        return;
    }

    private void FixedUpdate()
    {
        var velocity = controller.GetRigidbody().velocity;
        var isGround = controller.IsGround();
        animator.SetBool("IsGrounded", isGround);
        if (velocity.y < 0 && isGround)
        {
            if (inputVector.magnitude > 0)
                controller.ChangeState(PlayerStateType.Move);
            else
                controller.ChangeState(PlayerStateType.Idle);
        }

        controller.GetRigidbody().AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

    public void UpdateMoveInput(Vector3 inputVector)
    {
        if (!isStay)
            return;

        this.inputVector = inputVector;
    }

    public override void Exit()
    {
        isStay = false;

        exitEvent?.Invoke();
    }
}