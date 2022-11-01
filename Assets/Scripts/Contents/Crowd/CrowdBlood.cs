using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBlood : CrowdBehaviour
{
    [SerializeField]
    private float activeTime = .0f;
    [SerializeField]
    private int decreasePercent = 0;

    public override void Active()
    {
        // 플레이어 체력을 가져와서, Active 상태에서 일정한 시간 마다 피가 줄어들게 설정.

    }

    public override void UnActive()
    {

    }

    protected override void ApplyCrowd()
    {
        crowdType = CrowdType.Blood;

        currentLifeTime = activeTime;
        isActive = true;

    }
}
