using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : MonsterStateBase
{
    public override void Enter()
    {
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        enterEvent?.Invoke();
        gameObject.SetActive(false);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        return;
    }
}
