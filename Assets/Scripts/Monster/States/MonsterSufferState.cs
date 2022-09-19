using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSufferState : StateBase
{
    // 몬스터 넉백 시 애니메이션 변경 및 넉백 효과 적용
    public override void Action()
    {
        base.Action();

        Debug.Log("Suffer  State");

    }
}
