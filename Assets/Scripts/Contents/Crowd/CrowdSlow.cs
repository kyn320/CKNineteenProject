using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSlow : CrowdBehaviour
{
    /// <summary>
    /// ������ �̵��ӵ��� ���� �ð� ���� �����Ѵ�.
    /// ������ ���, ���� �ð��� ���� ���� �ð����� �ǵ�����.
    /// </summary>
    public float slowActiveCount = .0f;

    public override void Active()
    {
    }

    public override void UnActive()
    {
    }

    protected override void ApplyCrowd()
    {
        //playerController.GetStatus().SetCurrentStatus()
    }
}
