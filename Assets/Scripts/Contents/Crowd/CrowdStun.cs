using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdStun : CrowdBehaviour
{

    public override void Active()
    {
    }

    public override void UnActive()
    {
        playerController.GetInputController().enabled = true;
    }

    protected override void ApplyCrowd()
    {
        playerController.GetInputController().enabled = false;
    }
}
