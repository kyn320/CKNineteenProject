using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallGolemAttackPattern : MonsterAttackPattern
{
    [SerializeField]
    private float attackDistance = 0f;

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

        isAttacked = true;

        var status = controller.GetStatus();

        var attackSpeed = status.currentStatus.GetElement(StatusType.AttackSpeed).CalculateTotalAmount();
        var attackDistance = status.currentStatus.GetElement(StatusType.AttackDistance).CalculateTotalAmount();

        animator = controller.GetAnimator();
        for (var i = 0; i < startAttackTriggerDataList.Count; ++i)
        {
            startAttackTriggerDataList[i].Invoke(animator);
        }

        var destination = transform.position + transform.forward * attackDistance;

        //TODO :: 바라보고 있는 방향으로 직선 거리 n 만큼 이동.
        navAgent.speed = attackSpeed;
        navAgent.SetDestination(destination);

        startAttackEvent?.Invoke();
    }
    protected override void Update()
    {
        if (!isAttacked)
            return;

        if (navAgent.remainingDistance < 0.01f)
        {
            EndAttack();
        }
    }

    public override void EndAttack()
    {
        isAttacked = false;
        endAttackEvent?.Invoke();
    }


}
