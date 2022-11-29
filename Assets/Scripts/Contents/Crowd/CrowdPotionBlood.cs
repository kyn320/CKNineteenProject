using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPotionBlood : CrowdBehaviour
{
    /// <summary>
    /// 일정 시간마다 체력의 %비율이 지속적으로 감소됩니다.
    /// </summary>
    /// 

    [SerializeField]
    private float activeCalcTime = .0f;
    [SerializeField]
    private float activeStandardTime = .0f;
    [SerializeField]
    protected StatusCalculator statusCalculator;

    public float damageResult = 1f;


    public override void Active()
    {
        activeCalcTime -= Time.deltaTime;

        if (activeCalcTime <= 0)
        {
            string userTag = transform.parent.tag;


            if(userTag == "Monster")
            {
                damageResult = statusCalculator.Calculate(weaponData.StatusInfoData);
                Debug.Log($"damageResult : {damageResult}");

                monsterController?.OnDamage(new DamageInfo()
                {
                    hitPoint = monsterController.transform.position,
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
