using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSilent : CrowdBehaviour
{
    /// <summary>
    /// 침묵은 공격 키 입력 시에도 어떠한 반응도 존재하지 않는다.
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
