using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public class SmallGolemAttackPattern : MonsterAttackPattern
{
    [SerializeField]
    private float currentAttackTick = 0f;

    [SerializeField]
    private float attackTickTime = 0.1f;

    [SerializeField]
    private float attackAllowTime = 0.1f;

    [SerializeField]
    private bool allowAttack = false;

    [SerializeField]
    private float attackDistance = 0f;

    [SerializeField]
    private float attackAfterTime;

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

        //TODO :: �ٶ󺸰� �ִ� �������� ���� �Ÿ� n ��ŭ �̵�.
        navAgent.stoppingDistance = 0.01f;
        navAgent.speed = attackSpeed;
        var success = navAgent.SetDestination(destination);
        currentAttackTick = 0;
        startAttackEvent?.Invoke();
    }
    protected override void Update()
    {
        if (!isAttacked)
            return;

        currentAttackTick += Time.deltaTime;

        if (allowAttack && currentAttackTick >= attackAllowTime)
        {
            allowAttack = false;
            currentAttackTick = 0f;
        }
        else if (!allowAttack && currentAttackTick >= attackTickTime)
        {
            allowAttack = true;
            currentAttackTick = 0f;
        }

        if (navAgent.remainingDistance <= 0.1f && navAgent.velocity.sqrMagnitude > 0.1f)
        {
            EndAttack();
        }
    }

    public async override void EndAttack()
    {
        if (!isAttacked)
            return;

        isAttacked = false;

        navAgent.isStopped = true;

        animator = controller.GetAnimator();
        for (var i = 0; i < endAttackTriggerDataList.Count; ++i)
        {
            endAttackTriggerDataList[i].Invoke(animator);
        }

        endAttackEvent?.Invoke();
        await UniTask.Delay((int)(attackAfterTime * 1000));
        controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
            return;

        if (isAttacked)
        {
            if (!other.gameObject.CompareTag("Monster"))
            {
                Debug.Log("SmallGolemAttack :: " + other.gameObject + " / " + other.gameObject.tag);

                var damageable = other.gameObject.GetComponent<IDamageable>();

                var isCritical = controller.GetStatus().GetCriticalSuccess();
                var damageAmount = 0f;

                if (isCritical)
                {
                    damageAmount = damageCalculator.Calculate(controller.GetStatus().currentStatus);
                }
                else
                {
                    damageAmount = criticalDamageCalculator.Calculate(controller.GetStatus().currentStatus);
                }

                damageable?.OnDamage(new DamageInfo()
                {
                    damage = damageAmount,
                    isCritical = isCritical,
                    isKnockBack = true,
                    hitPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position),
                    hitNormal = transform.position - other.transform.position,
                });

            }
            //else {
            //    EndAttack();
            //}
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }
}
