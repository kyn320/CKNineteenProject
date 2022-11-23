using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

public class BigGolemTwoHandAttackPattern : MonsterAttackPattern
{
    private NavMeshAgent navAgent;

    [SerializeField]
    private GameObject groundHitBox;

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

        Debug.Log("Attack_BigGolem Two Hand");

        this.target = target;
        isAttacked = true;

        navAgent.isStopped = true;

        animator = controller.GetAnimator();
        for (var i = 0; i < startAttackTriggerDataList.Count; ++i)
        {
            startAttackTriggerDataList[i].Invoke(animator);
        }


        startAttackEvent?.Invoke();

        //임시로 SpawnGroundHitBox를 이곳에서 부르도록 만들어 두었습니다. 확인 후 적절한곳에 베치해주신다면 감사드리겠습니다.
        SpawnGroundHitBox();
    }

    protected override void Update()
    {
        return;
    }

    public async void SpawnGroundHitBox()
    {
        await UniTask.Delay(1250);
        Debug.Log($"vfxPrefabData.GetVFXPrefab(Attack_1) : {vfxPrefabData.GetVFXPrefab("Attack_1").name}");
        GameObject effect = Instantiate(vfxPrefabData.GetVFXPrefab("Attack_1"), transform);
        effect.transform.parent = null;
        groundHitBox.SetActive(true);
        await UniTask.Delay(1000);
        groundHitBox.SetActive(false);
    }

    public void HitAttack(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
            return;

        if (isAttacked)
        {
            if (!collider.gameObject.CompareTag("Monster"))
            {
                Debug.Log("BigGolemTwoHandAttack :: " + collider.gameObject + " / " + collider.gameObject.tag);

                var damageable = collider.gameObject.GetComponent<IDamageable>();

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
                var closetPoint = collider.ClosestPoint(transform.position);
                damageable?.OnDamage(new DamageInfo()
                {
                    damage = damageAmount,
                    isCritical = isCritical,
                    isKnockBack = true,
                    hitPoint = closetPoint,
                    hitNormal = transform.position - closetPoint
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
