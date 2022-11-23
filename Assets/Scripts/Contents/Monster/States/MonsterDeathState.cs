using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : MonsterStateBase
{
    public override void Enter()
    {
        enterEvent?.Invoke();

        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        Invoke("Delete", 5f);
    }

    void Delete()
    {
        Destroy(gameObject);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        return;
    }
}
