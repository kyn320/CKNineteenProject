using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCriticalHitState : PlayerStateBase
{
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
        return;
    }

    public override void Exit()
    {
        isStay = false;
        for (var i = 0; i < exitAnimatorTriggerList.Count; ++i)
        {
            exitAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }
        exitEvent?.Invoke();
    }
    public void EndHit()
    {
        if (isStay)
            controller.ChangeState(PlayerStateType.Idle);
    }
}