using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : StateBase
{
    private SerializableDictionary<StatusType, StatusElement> monsterStatus;
    private float idleTime = 0.0f;
    private float idleCountTime = 0.0f;

    private Collider[] coll;

    public bool debugMode = false;

    public override void Action()
    {
        base.Action();

        idleCountTime = 0.0f;
        idleTime = Random.Range(1f, 3f);
        monsterStatus = manager.monster.monsterStatus.StausDic;
    }

    private void Update()
    {
        idleCountTime += Time.deltaTime;

        coll = Physics.OverlapSphere(transform.localPosition, monsterStatus[StatusType.SightDistance].GetAmount());

        for (int i = 0; i < coll.Length; i++)
        {
            if (coll[i].CompareTag("Player"))
            {
                manager.SetTarget(coll[i].gameObject);
                var dirToTarget = (manager.GetTargetPosition() - transform.position).normalized;
                
                // ���߿� Object Check�� ���ְ�, Object Y Pos�� Monster Sight... Check ���ָ� Object Y Pos ���� ����

                if (Vector3.Angle(transform.forward, dirToTarget) < monsterStatus[StatusType.SightDegree].GetAmount() / 2)
                {
                    manager.PlayAction(MonsterState.MONSTERSTATE_TRACKING);
                    return;
                }
            }
        }

        if (idleCountTime >= idleTime)
            manager.PlayAction(MonsterState.MONSTERSTATE_PATROLL);
    }
}

