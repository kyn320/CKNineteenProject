using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdStun : CrowdBehaviour
{
    /// <summary>
    /// ���� ���¿����� ��� �ൿ�� �� �� ����.
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
