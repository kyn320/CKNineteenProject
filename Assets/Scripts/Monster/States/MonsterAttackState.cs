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
        // 애니메이션이 종료되었을 경우, 공격 효과 적용.
        Debug.Log("attack anim end");
    }

    public void SetTrackingAction()
    {
        manager.PlayAction(MonsterState.MONSTERSTATE_TRACKING);
    }


    //애니메이션이 끝나고 플레이어가 옆에 있으면 다시 공격
    //플레이어가 옆에 없으면 Tracking 그러나 범위 밖이면 Idle.

}
