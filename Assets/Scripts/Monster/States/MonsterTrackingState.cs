using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrackingState : StateBase
{
    // 반경 N에 있을 경우, 플레이어 추적
    // 반경 M에 들어왔을 경우, 플레이어 공격 
    public override void Action()
    {
        base.Action();

        Debug.Log("Monster State : Tracking");
    }

    private void Update()
    {
        Vector3 targetPosition = manager.GetTargetPosition();

        // 몬스터의 회전 각을 플레이어를 바라볼 수 있도록 설정 후
        // 몬스터를 Forward 시켜서 다가가게 설정한다.

        // 그러다가, 플레이어가 일정 영역에 존재하지 않으면 Patroll 혹은 idle로 설정을 전환한다.

        
    }
}
