using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }
        isStay = true;
        controller.updateMoveSpeedEvent?.Invoke(1f);
        enterEvent?.Invoke();
        Jump();
    }

    public override void Update()
    {
        return;
    }

    public void Jump()
    {
        var animator = controller.GetAnimator();
        var status = controller.GetStatus();

        animator.SetBool("IsGrounded", false);

        var jumpPower = status.currentStatus.GetElement(StatusType.JumpPower).CalculateTotalAmount();
        var velocity = controller.GetRigidbody().velocity;
        velocity.y = jumpPower;
        controller.GetRigidbody().velocity = velocity;
        controller.ChangeState(PlayerStateType.Air);
    }

    public override void Exit()
    {
        isStay = false;

        exitEvent?.Invoke();
    }


}