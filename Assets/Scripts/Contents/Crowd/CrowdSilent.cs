using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSilent : CrowdBehaviour
{
    /// <summary>
    /// ħ���� ���� Ű �Է� �ÿ��� ��� ������ �������� �ʴ´�.
    /// </summary>
    public override void Active()
    {
    }

    public override void UnActive()
    {
        playerController.SetStateEnbaled(PlayerStateType.Attack, true);
    }

    protected override void ApplyCrowd()
    {
        playerController.SetStateEnbaled(PlayerStateType.Attack, false);
    }
}
