using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrackingState : StateBase
{
    // �ݰ� N�� ���� ���, �÷��̾� ����
    // �ݰ� M�� ������ ���, �÷��̾� ���� 
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
        manager.rig.velocity = new Vector3(movePos.x * manager.monster.GetMoveSpeed(), movePos.y, movePos.z * manager.monster.GetMoveSpeed());

        if (targetPos.sqrMagnitude < 1f)
        {
            manager.PlayAction(MonsterState.MONSTERSTATE_ATTACK);
        } else if(targetPos.sqrMagnitude > manager.monster.GetSearchRadius() + 3f)
        {
            var randNum = Random.Range(0, 2);

            if (randNum == 0)
                manager.PlayAction(MonsterState.MONSTERSTATE_IDLE);
            else
                manager.PlayAction(MonsterState.MONSTERSTATE_PATROLL);
        }

    }
}
