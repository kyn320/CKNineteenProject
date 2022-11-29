using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : MonsterStateBase
{
    [SerializeField]
    private float deathDelay = .0f;
    public override void Enter()
    {
        enterEvent?.Invoke();

        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        Invoke("Delete", deathDelay);
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
