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
    private float activeCalcTime = .0f;
    private float activeStandardTime = .0f;

    public override void Active()
    {
        activeCalcTime -= Time.deltaTime;

        if (activeCalcTime <= 0)
        {
            var damageElement = GetBuffData().GetStatusElement(StatusType.HP);

            string userTag = transform.parent.tag;
            if (userTag == "Player")
            {
                float damageResult = playerController.GetStatus().currentStatus.GetElement(StatusType.HP).CalculateTotalAmount()
                    * (damageElement.GetPercent() / 100f);

                playerController?.OnDamage(new DamageInfo()
                {
                    damage = damageResult,
                    isCritical = false,
                    isKnockBack = false
                });
            } else if(userTag == "Monster")
            {
                float damageResult = monsterController.GetStatus().currentStatus.GetElement(StatusType.HP).CalculateTotalAmount()
                    * (damageElement.GetPercent() / 100f);


                Debug.Log($"damageResult : {damageResult}");
                monsterController?.OnDamage(new DamageInfo()
                {
                    damage = damageResult,
                    isCritical = false,
                    isKnockBack = false
                });
            }

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
