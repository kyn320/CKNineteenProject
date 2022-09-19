using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : StateBase
{
    private Vector3 attackPos;
    public override void Action()
    {
        base.Action();

        attackPos = manager.GetTargetPosition();
    }

    public void Attack()
    {
        // �ִϸ��̼��� ����Ǿ��� ���, ���� ȿ�� ����.
        Debug.Log("attack anim end");
    }

    public void SetTrackingAction()
    {
        manager.PlayAction(MonsterState.MONSTERSTATE_TRACKING);
    }


    //�ִϸ��̼��� ������ �÷��̾ ���� ������ �ٽ� ����
    //�÷��̾ ���� ������ Tracking �׷��� ���� ���̸� Idle.

}
