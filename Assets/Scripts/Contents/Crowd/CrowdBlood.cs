using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBlood : CrowdBehaviour
{
    [SerializeField]
    private float activeTime = .0f;
    [SerializeField]
    private int decreasePercent = 0;

    private float playerHp = .0f;

    public override void Active()
    {
        // OnDamaage�� ���ؼ�, ���� �ð��� ������ 

    }

    public override void UnActive()
    {

    }

    protected override void ApplyCrowd()
    {
        currentLifeTime = activeTime;

        // �ʱ� �÷��̾� HP ��������.

    }
}
