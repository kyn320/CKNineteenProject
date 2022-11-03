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
    private float activeCalcTime = .0f;
    private float activeStandardTime = .0f;

    public override void Active()
    {
        activeCalcTime -= Time.deltaTime;

        if (activeCalcTime <= 0)
        {
            var damageElement = GetBuffData().GetStatusElement(StatusType.HP);
            float damageResult = playerController.GetStatus().currentStatus.GetElement(StatusType.HP).CalculateTotalAmount() * damageElement.GetPercent();

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
