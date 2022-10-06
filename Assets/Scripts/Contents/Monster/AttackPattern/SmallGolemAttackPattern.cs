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

        this.target = target;
        isAttacked = true;

        var status = controller.GetStatus();

        var attackSpeed = status.currentStatus.GetElement(StatusType.AttackSpeed).CalculateTotalAmount();
        attackDistance = status.currentStatus.GetElement(StatusType.AttackDistance).CalculateTotalAmount();

        animator = controller.GetAnimator();
        for (var i = 0; i < startAttackTriggerDataList.Count; ++i)
        {
            startAttackTriggerDataList[i].Invoke(animator);
        }

        var destination = transform.position + transform.forward * attackDistance;

        Debug.Log(Vector3.Distance(destination, transform.position));

        //TODO :: 바라보고 있는 방향으로 직선 거리 n 만큼 이동.
        navAgent.stoppingDistance = 0.01f;
        navAgent.speed = attackSpeed;
        var success = navAgent.SetDestination(destination);
        Debug.Log(success);

        startAttackEvent?.Invoke();
    }
    protected override void Update()
    {
        if (!isAttacked)
            return;

        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            EndAttack();
        }
    }

    public override void EndAttack()
    {
        isAttacked = false;

        animator = controller.GetAnimator();
        for (var i = 0; i < endAttackTriggerDataList.Count; ++i)
        {
            endAttackTriggerDataList[i].Invoke(animator);
        }

        controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);

        endAttackEvent?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isAttacked)
            EndAttack();
    }
}
