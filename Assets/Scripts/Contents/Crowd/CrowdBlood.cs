using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBlood : CrowdBehaviour
{
    /// <summary>
    /// 일정 시간마다 체력의 %비율이 지속적으로 감소됩니다.
    /// </summary>
    /// 
    [SerializeField]
    private int damagePercent = 0;

    [SerializeField]
    private float activeCalcTime = .0f;
    private float activeStandardTime = .0f;

    public override void Active()
    {
        activeCalcTime -= Time.deltaTime;

        if (activeCalcTime <= 0)
        {
            float damageResult = (playerController.GetStatus().currentStatus.GetElement(StatusType.HP).GetAmount() / 100) * damagePercent;

            playerController?.OnDamage(new DamageInfo()
            {
                damage = damageResult,
                isCritical = false,
                isKnockBack = false
            });
            activeCalcTime = activeStandardTime;
        }
        
    }

    public override void UnActive()
    {

    }

    protected override void ApplyCrowd()
    {

    }
}
