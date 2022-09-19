using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : StateBase
{
    private float idleTime = 0.0f;
    private float idleCountTime = 0.0f;

    [SerializeField]
    private Collider[] coll;

    public override void Action()
    {
        base.Action();

        idleCountTime = 0.0f;
        idleTime = Random.Range(1f, 3f);
    }

    private void Update()
    {
        idleCountTime += Time.deltaTime;

        coll = Physics.OverlapSphere(transform.localPosition, manager.monster.GetSearchRadius());

        for (var i = 0; i < coll.Length; i++)
        {
            if (coll[i].gameObject.name == "Player")
            {
                Debug.Log("Player Check");
                manager.SetTarget(coll[i].gameObject);
                manager.PlayAction(MonsterState.MONSTERSTATE_TRACKING);
            }
            else
            {
                if (idleCountTime > idleTime)
                {
                    Debug.Log("Idle Time Over");
                    manager.PlayAction(MonsterState.MONSTERSTATE_PATROLL);
                }
            }

        }

    }
}
