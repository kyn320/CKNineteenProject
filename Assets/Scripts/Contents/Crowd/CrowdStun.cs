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
        string userTag = transform.parent.tag;
        if (userTag == "Player")
        {
            playerController.GetInputController().enabled = true;
        }
        else if (userTag == "Monster")
        {

        }
    }

    protected override void ApplyCrowd()
    {
        playerController.GetInputController().enabled = false;
    }
}
