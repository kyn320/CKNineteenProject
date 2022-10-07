using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerStateBase
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

        exitEvent?.Invoke();
    }
}