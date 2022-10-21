using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSlow : CrowdBehaviour
{
    public float slowActiveCount = .0f;

    public override void Active()
    {
    }

    public override void UnActive()
    {
    }

    protected override void ApplyCrowd()
    {
        crowdType = CrowdType.Slow;

        //playerController.GetStatus().SetCurrentStatus()
    }
}
