using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrollState : StateBase
{
    // N 초 동안 추적하다가 Idle로 변경 
    // Player를 발견했을 경우, Attack으로 전환

    public override void Action()
    {
        base.Action();

        Debug.Log("Monster Patroll");
    
    }
}
