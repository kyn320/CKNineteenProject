using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        var animator = controller.GetAnimator();
        animator.SetInteger("MoveSpeed", 0);
        animator.SetBool("IsGrounded", true);

        isStay = true;
        controller.updateMoveSpeedEvent?.Invoke(0f);

        enterEvent?.Invoke();
    }

    public void UpdateMoveInput(Vector3 inputVector)
    {
        if(!isStay)
            return;

        if (inputVector.magnitude > 0)
            controller.ChangeState(PlayerStateType.Move);
    }

    public void UpdateForwardView(Vector3 forwardView)
    {
        if (!isStay)
            return;

        forwardView.y = 0;
        transform.forward = forwardView;
    }

    public override void Update()
    {
        //TODO :: Auto Recover HP per Time(1 sec)

    }

    private void FixedUpdate()
    {
        controller.GetRigidbody().velocity = Vector3.zero;
    }

    public override void Exit()
    {
        isStay = false;
        controller.updateMoveSpeedEvent?.Invoke(1f);
        exitEvent?.Invoke();
    }
}
