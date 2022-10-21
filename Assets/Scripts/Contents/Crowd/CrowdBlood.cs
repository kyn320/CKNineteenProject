using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBlood : CrowdBehaviour
{
    [SerializeField]
    private float activeTime = .0f;
    [SerializeField]
    private int decreasePercent = 0;

    private float playerHp = .0f;

    public override void Active()
    {
        // OnDamaage를 통해서, 일정 시간이 지나면 

    }

    public override void UnActive()
    {

    }

    protected override void ApplyCrowd()
    {
        currentLifeTime = activeTime;

        // 초기 플레이어 HP 가져오기.

    }
}
