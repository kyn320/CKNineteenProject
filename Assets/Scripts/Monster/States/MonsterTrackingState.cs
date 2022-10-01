using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrackingState : StateBase
{
    public override void Action()
    {
        base.Action();
    }

    private void FixedUpdate()
    {
        var targetPos = manager.GetTargetPosition() - transform.localPosition;
        manager.agent.SetDestination(manager.GetTargetPosition());

        Collider[] coll = Physics.OverlapSphere(transform.localPosition, 1f);

        for (var i = 0; i < coll.Length; i++)
        {
            if (coll[i].CompareTag("Field"))
            {
                manager.PlayAction(MonsterState.MONSTERSTATE_ATTACK);
            }
        }

        if (targetPos.sqrMagnitude < 1f)
        {
            manager.PlayAction(MonsterState.MONSTERSTATE_ATTACK);
        }
    }
}
