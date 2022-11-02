using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSlow : CrowdBehaviour
{
    /// <summary>
    /// 지연은 이동속도가 일정 시간 동안 감소한다.
    /// 누적의 경우, 지속 시간을 최초 지속 시간으로 되돌린다.
    /// </summary>
    [SerializeField]
    private float slowPercent = .0f;

    [SerializeField]
    private float slowSpeed = .0f;
    private float playerSpeed = 0f;


    public override void Active()
    {
    }

    public override void UnActive()
    {
        playerController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).SetAmount(playerSpeed);
    }

protected override void ApplyCrowd()
    {
        playerSpeed = playerController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).GetAmount();
        slowSpeed = (playerSpeed / 100) * (100 - slowPercent);

        playerController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).SetAmount(slowSpeed);
    }
}
