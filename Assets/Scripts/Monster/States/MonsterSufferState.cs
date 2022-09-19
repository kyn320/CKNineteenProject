using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSufferState : StateBase
{
    // 넉백 효과 적용
    public override void Action()
    {
        base.Action();

        manager.rig.velocity = new Vector3(transform.localPosition.x - 1, transform.localPosition.y, transform.localPosition.z - 1);
        manager.PlayAction(MonsterState.MONSTERSTATE_TRACKING);
    }
}
