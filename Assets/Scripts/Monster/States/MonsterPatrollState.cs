using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrollState : StateBase
{
    // N �� ���� �����ϴٰ� Idle�� ���� 
    // Player�� �߰����� ���, Attack���� ��ȯ

    public override void Action()
    {
        base.Action();

        Debug.Log("Monster Patroll");
    
    }
}
