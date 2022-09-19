using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : StateBase
{
    public override void Action()
    {
        base.Action();

        Debug.Log("Attack State");
    }
}
