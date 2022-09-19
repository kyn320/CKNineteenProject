using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : StateBase
{
    public override void Action()
    {
        base.Action();
        // 아이템 드랍 구현

        //Destroy(this);
    }
}
