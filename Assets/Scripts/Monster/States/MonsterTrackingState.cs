using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrackingState : StateBase
{
    // �ݰ� N�� ���� ���, �÷��̾� ����
    // �ݰ� M�� ������ ���, �÷��̾� ���� 
    public override void Action()
    {
        base.Action();

        Debug.Log("Monster State : Tracking");
    }

    private void Update()
    {
        Vector3 targetPosition = manager.GetTargetPosition();

        // ������ ȸ�� ���� �÷��̾ �ٶ� �� �ֵ��� ���� ��
        // ���͸� Forward ���Ѽ� �ٰ����� �����Ѵ�.

        // �׷��ٰ�, �÷��̾ ���� ������ �������� ������ Patroll Ȥ�� idle�� ������ ��ȯ�Ѵ�.

        
    }
}
