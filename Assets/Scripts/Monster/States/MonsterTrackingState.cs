using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrackingState : StateBase
{
    public override void Action()
    {
        base.Action();
    }

    private void FixedUpdate()
    {
        var targetPos = manager.GetTargetPosition() - transform.localPosition;
        targetPos.y = 0;

        var rotation = Quaternion.LookRotation(targetPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);

        var movePos = targetPos.normalized;
        manager.rig.velocity = new Vector3(movePos.x * manager.monster.monsterStatus.StausDic[StatusType.MoveSpeed].GetAmount(), movePos.y, movePos.z * manager.monster.monsterStatus.StausDic[StatusType.MoveSpeed].GetAmount());

        if (targetPos.sqrMagnitude < 1f)
        {
            manager.PlayAction(MonsterState.MONSTERSTATE_ATTACK);
        }
    }
}
