using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSilent : CrowdBehaviour
{
    /// <summary>
    /// ħ���� ���� Ű �Է� �ÿ��� ��� ������ �������� �ʴ´�.
    /// </summary>
    public override void Active()
    {


    }

    public override void UnActive()
    {

    }

    protected override void ApplyCrowd()
    {
        if (Input.GetMouseButtonDown(0))
            return;
    }
}
