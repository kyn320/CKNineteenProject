using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterHitState : MonsterStateBase
{
    protected Rigidbody rigid;
    protected NavMeshAgent navAgent;
    [SerializeField]
    protected Vector3 knockBackVector;
    [SerializeField]
    protected VFXPrefabData vfxPrefabData;

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
        isStay = true;
        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }
        enterEvent?.Invoke();
    }

    public override void Exit()
    {
        isStay = false;
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

    public virtual void DamageHit(DamageInfo damageInfo)
    {

    }
}
