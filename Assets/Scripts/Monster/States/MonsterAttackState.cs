using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : StateBase
{
    private Vector3 attackPos;
    public override void Action()
    {
        base.Action();
    }

    //애니메이션이 끝나고 플레이어가 옆에 있으면 다시 공격
    // 플레이어가 옆에 없으면 Tracking 그러나 범위 밖이면 Idle.

}
