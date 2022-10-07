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

        animator.SetTrigger("Jump");
        animator.SetBool("IsGrounded", false);

        var moveVector = controller.GetMoveVector();
        moveVector.y = status.currentStatus.GetElement(StatusType.JumpPower).CalculateTotalAmount();

        controller.SetMoveVector(moveVector);
        controller.ChangeState(PlayerStateType.Air);
    }

    public override void Exit()
    {
        isStay = false;

        exitEvent?.Invoke();
    }


}