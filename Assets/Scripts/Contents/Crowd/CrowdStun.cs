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
        Debug.Log("STUN START");
    }

    public override void UnActive()
    {
        Debug.Log("STUN END");
        playerController.GetInputController().enabled = true;
    }

    protected override void ApplyCrowd()
    {
        Debug.Log("APPLY CRWOD");
        playerController.GetInputController().enabled = false;
    }
}
