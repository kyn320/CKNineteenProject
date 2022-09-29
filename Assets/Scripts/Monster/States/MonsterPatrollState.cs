using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrollState : StateBase
{
    private Vector3 arrivalPos;

    [SerializeField]
    private Collider[] coll;

    public override void Action()
    {
        base.Action();

        arrivalPos = new Vector3(Random.Range(-10, 11), 0, Random.Range(-10, 11));

    }

    private void Update()
    {
        coll = Physics.OverlapSphere(transform.localPosition, manager.monster.monsterStatus.StausDic[StatusType.SightDistance].GetAmount());
        
        for (var i = 0; i < coll.Length; i++)
        {
            if (coll[i].gameObject.name == "Player")
            {
                manager.SetTarget(coll[i].gameObject);
                manager.PlayAction(MonsterState.MONSTERSTATE_TRACKING);
            }
            else
            {
                var targetPos = arrivalPos - transform.localPosition;
                targetPos.y = 0;

                var rotation = Quaternion.LookRotation(targetPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);

                var movePos = targetPos.normalized;
                manager.rig.velocity = new Vector3(movePos.x * manager.monster.monsterStatus.StausDic[StatusType.MoveSpeed].GetAmount(), movePos.y, movePos.z * manager.monster.monsterStatus.StausDic[StatusType.MoveSpeed].GetAmount());

                if (targetPos.sqrMagnitude < 1f)
                {
                    var randNum = Random.Range(0, 2);

                    if (randNum == 0)
                        manager.PlayAction(MonsterState.MONSTERSTATE_IDLE);
                    else
                        manager.PlayAction(MonsterState.MONSTERSTATE_PATROLL);
                }
            }

        }
    }
}
