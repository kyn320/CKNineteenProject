using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSlow : CrowdBehaviour
{
    /// <summary>
    /// ������ �̵��ӵ��� ���� �ð� ���� �����Ѵ�.
    /// ������ ���, ���� �ð��� ���� ���� �ð����� �ǵ�����.
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
