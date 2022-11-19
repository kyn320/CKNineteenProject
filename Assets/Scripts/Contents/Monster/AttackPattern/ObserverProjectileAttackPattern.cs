using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObserverProjectileAttackPattern : MonsterAttackPattern
{
    [SerializeField]
    private Transform renderObject;

    [SerializeField]
    private Transform shotPoint;

    [SerializeField]
    private float attackDistance = 0f;

    private NavMeshAgent navAgent;

    [SerializeField]
    private GameObject projectilePrefab;

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

        navAgent.isStopped = true;
        startAttackEvent?.Invoke();

        Instantiate(vfxPrefabData.GetVFXPrefab("ProjectileAttack"), shotPoint);
    }

    public void PlayAttack()
    {
        if (!isAttacked)
            return;

        //TODO :: Projectile Shot
        var projectileObject = Instantiate(projectilePrefab);

        var projectileDirection = target.position - shotPoint.position;

        var projectileController = projectileObject.GetComponent<ProjectileController>();

        var status = controller.GetStatus();

        projectileController.SetCalculator(CalculateCritical, CalculateDamageAmount);

        projectileController.Shot(shotPoint.position
            , target.position
            , projectileDirection.normalized
            , status.currentStatus.GetElement(StatusType.ThrowSpeed).GetAmount()
            , attackDistance);
    }

    protected override void Update()
    {
        return;
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

        endAttackEvent?.Invoke();
        controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);
    }
    private bool CalculateCritical()
    {
        return controller.GetStatus().GetCriticalSuccess();
    }

    private float CalculateDamageAmount(bool isCritical)
    {
        if (isCritical)
        {
            return criticalDamageCalculator.Calculate(controller.GetStatus().currentStatus);
        }
        else
        {
            return damageCalculator.Calculate(controller.GetStatus().currentStatus);
        }
    }

}
