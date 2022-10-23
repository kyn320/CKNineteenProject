using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBlood : CrowdBehaviour
{
    [SerializeField]
    private float activeTime = .0f;
    [SerializeField]
    private int decreasePercent = 0;

    public override void Active()
    {
        // �÷��̾� ü���� �����ͼ�, Active ���¿��� ������ �ð� ���� �ǰ� �پ��� ����.

    }

    public override void UnActive()
    {

    }

    protected override void ApplyCrowd()
    {
        crowdType = CrowdType.Blood;

        currentLifeTime = activeTime;
        isActive = true;

    }
}
