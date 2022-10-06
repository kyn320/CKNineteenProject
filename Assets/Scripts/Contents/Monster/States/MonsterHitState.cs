using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterHitState : MonsterStateBase
{
    protected Rigidbody rigid;
    protected NavMeshAgent navAgent;
    protected Vector3 knockBackVector;

    protected override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // 넉백 효과 적용
    public override void Enter()
    {
        navAgent.enabled = false;
        enterEvent?.Invoke();
    }

    public override void Exit()
    {
        navAgent.enabled = true;
        exitEvent?.Invoke();
    }

    public override void Update()
    {
        return;
    }

    public void KnockBack()
    {
        rigid.velocity = knockBackVector;
    }

    public void EndHit()
    {
        controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);
    }
}
