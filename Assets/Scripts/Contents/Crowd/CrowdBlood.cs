using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBlood : CrowdBehaviour
{
    /// <summary>
    /// ���� �ð����� ü���� %������ ���������� ���ҵ˴ϴ�.
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
