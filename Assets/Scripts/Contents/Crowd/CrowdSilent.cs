using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSilent : CrowdBehaviour
{
    public override void Active()
    {


    }

    public override void UnActive()
    {

    }

    protected override void ApplyCrowd()
    {
        crowdType = CrowdType.Slient;

        if (Input.GetMouseButtonDown(0))
            return;
    }
}
