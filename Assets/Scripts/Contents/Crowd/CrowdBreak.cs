using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBreak : CrowdBehaviour
{
    private float playerDefence;

    public override void Active()
    {
    }

    public override void UnActive()
    {
        playerController.GetStatus().currentStatus.GetElement(StatusType.Defence).SetAmount(playerDefence);
    }

    protected override void ApplyCrowd()
    {
        playerDefence = playerController.GetStatus().currentStatus.GetElement(StatusType.Defence).GetAmount();

        playerController.GetStatus().currentStatus.GetElement(StatusType.Defence).SetAmount(0);
    }
}
