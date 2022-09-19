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

    private void Update()
    {
        // 반경을 불러와서 그 주위를 순찰한다.
        // 순찰 시간을 정해서, 그 시간이 종료 되었을 때, idle로 전환한다.
        // 단, 반경 안에 플레이어가 있을 경우, Tracking으로 전환한다.


    }
}
