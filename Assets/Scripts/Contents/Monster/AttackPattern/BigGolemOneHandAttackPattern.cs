using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigGolemOneHandAttackPattern : MonsterAttackPattern
{
    private NavMeshAgent navAgent;

    [SerializeField]
    private VFXPrefabData vfxPrefabData;

    [SerializeField]
    private SFXPrefabData sfxPrefabData;

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
        return;
    }

    public void HitAttack(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            return;

        if (isAttacked)
        {
            if (!collision.gameObject.CompareTag("Monster"))
            {
                Debug.Log("BigGolemOneHandAttack :: " + collision.gameObject + " / " + collision.gameObject.tag);

                var damageable = collision.gameObject.GetComponent<IDamageable>();

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
                    hitPoint = collision.contacts[0].point,
                    hitNormal = collision.contacts[0].normal
                });
            }
        }
    }

    public override void EndAttack()
    {
        if (!isAttacked)
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
