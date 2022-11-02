using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class MonsterChaseState : MonsterStateBase
{
    [SerializeField]
    private AmountRangeFloat refreshPathTimeRange;

    [SerializeField]
    private float refreshPathTime;

    [SerializeField]
    private float stopDistance = 1f;
    [SerializeField]
    private float attackStartDistance = 1f;
    [SerializeField]
    private float attackStartViewDegree = 10f;
    [SerializeField]
    private float currentDistanceSqr = 0f;

    [SerializeField]
    private NavMeshAgent navAgent;

    [ShowInInspector]
    [ReadOnly]
    private FieldOfView fieldOfView;

    protected override void Awake()
    {
        base.Awake();
        refreshPathTime = refreshPathTimeRange.GetRandomAmount();
        navAgent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<FieldOfView>();
    }

    public override void Enter()
    {
        var target = controller.GetTarget();

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            controller.SetTarget(target);
        }

        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        var status = controller.GetStatus();
        navAgent.speed = status.currentStatus.GetElement(StatusType.MoveSpeed).CalculateTotalAmount();
        navAgent.stoppingDistance = stopDistance;

        RefreshPathTime();
        navAgent.isStopped = false;
        navAgent.SetDestination(controller.GetTarget().position);

        enterEvent?.Invoke();
    }

    public override void Exit()
    {
        exitEvent?.Invoke();
    }

    public override void Update()
    {
        var target = controller.GetTarget();

        refreshPathTime -= Time.deltaTime;

        if (refreshPathTime <= 0f)
        {
            RefreshPathTime();
            navAgent.SetDestination(target.position);
        }

        if (fieldOfView.CheckFov(target, attackStartViewDegree, attackStartDistance))
        {
            controller.ChangeState(MonsterStateType.MONSTERSTATE_ATTACK);
        }
    }

    public void RefreshPathTime()
    {
        refreshPathTime = refreshPathTimeRange.GetRandomAmount();
    }
}
