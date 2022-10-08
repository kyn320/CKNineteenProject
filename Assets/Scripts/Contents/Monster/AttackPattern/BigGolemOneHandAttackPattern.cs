using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigGolemOneHandAttackPattern : MonsterAttackPattern
{
    private NavMeshAgent navAgent;

    protected override void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavMeshAgent>();    
    }

    public override void StartAttack(Transform target)
    {
        if (isAttacked)
            return;

        Debug.Log("Attack_BigGolem One Hand");

        this.target = target;
        isAttacked = true;

        navAgent.isStopped = true;

        animator = controller.GetAnimator();
        for (var i = 0; i < startAttackTriggerDataList.Count; ++i)
        {
            startAttackTriggerDataList[i].Invoke(animator);
        }
        startAttackEvent?.Invoke();
    }

    protected override void Update()
    {

    }

    public void HitAttack() { 
        
    }

    public override void EndAttack()
    {
        if(!isAttacked)
            return;

        isAttacked = false;

        animator = controller.GetAnimator();
        for (var i = 0; i < endAttackTriggerDataList.Count; ++i)
        {
            endAttackTriggerDataList[i].Invoke(animator);
        }

        navAgent.isStopped = false;
        controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);

        endAttackEvent?.Invoke();
    }
}
