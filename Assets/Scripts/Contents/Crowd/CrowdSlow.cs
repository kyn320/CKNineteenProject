using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSlow : CrowdBehaviour
{
    /// <summary>
    /// 지연은 이동속도가 일정 시간 동안 감소한다.
    /// 누적의 경우, 지속 시간을 최초 지속 시간으로 되돌린다.
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
