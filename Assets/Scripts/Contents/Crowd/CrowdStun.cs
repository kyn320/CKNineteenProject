using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdStun : CrowdBehaviour
{
    /// <summary>
    /// 스턴 상태에서는 어떠한 행동도 할 수 없다.
    /// </summary>
    public override void Active()
    {
        playerController.GetInputController().enabled = false;
    }

    public override void UnActive()
    {
        playerController.GetInputController().enabled = true;
    }

    protected override void ApplyCrowd()
    {
    }
}
