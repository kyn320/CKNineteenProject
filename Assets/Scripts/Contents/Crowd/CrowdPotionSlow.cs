using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPotionSlow : CrowdBehaviour
{
    /// <summary>
    /// ���� �ð����� ü���� %������ ���������� ���ҵ˴ϴ�.
    /// </summary>
    /// 

    [SerializeField]
    private float activeCalcTime = 0;
    [SerializeField]
    private float activeStandardTime = 1;
    [SerializeField]
    protected StatusCalculator statusCalculator;

    public float damageResult = 1f;

    
    [SerializeField]
    private float slowPercent = 60f;

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
        monsterController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).SetAmount(userSpeed);
    }

    protected override void ApplyCrowd()
    {
        userSpeed = monsterController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).GetAmount();
        slowSpeed = (userSpeed / 100) * (100 - slowPercent);

        monsterController.GetStatus().currentStatus.GetElement(StatusType.MoveSpeed).SetAmount(slowSpeed);
    }
}
