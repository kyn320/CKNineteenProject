using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPotionSlow : CrowdBehaviour
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

    
    [SerializeField]
    private float slowPercent = 50f;

    [SerializeField]
    private float slowSpeed = .0f;
    private float userSpeed = 0f;


    public override void Active()
    {
        activeCalcTime -= Time.deltaTime;

        if (activeCalcTime <= 0)
        {
            string userTag = transform.parent.tag;


            if (userTag == "Monster")
            {
                damageResult = statusCalculator.Calculate(weaponData.StatusInfoData);
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
        monsterController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).SetAmount(userSpeed);
    }

    protected override void ApplyCrowd()
    {
        userSpeed = monsterController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).GetAmount();
        slowSpeed = (userSpeed / 100) * (100 - slowPercent);

        monsterController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).SetAmount(slowSpeed);
    }
}
