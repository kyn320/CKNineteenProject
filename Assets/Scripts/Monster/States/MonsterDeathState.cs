using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : StateBase
{
    // 몬스터 사망 시, 아이템 드랍 구현
    public override void Action()
    {
        base.Action();

        Debug.Log("Death State");
    }
}
