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

    private void Update()
    {
        // �ݰ��� �ҷ��ͼ� �� ������ �����Ѵ�.
        // ���� �ð��� ���ؼ�, �� �ð��� ���� �Ǿ��� ��, idle�� ��ȯ�Ѵ�.
        // ��, �ݰ� �ȿ� �÷��̾ ���� ���, Tracking���� ��ȯ�Ѵ�.


    }
}
