using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : StateBase
{
    // ���� ��� ��, ������ ��� ����
    public override void Action()
    {
        base.Action();

        Debug.Log("Death State");
    }
}
