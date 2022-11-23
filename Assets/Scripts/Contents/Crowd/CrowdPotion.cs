using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPotion : CrowdBehaviour
{
    /// <summary>
    /// ���� �ð����� ü���� %������ ���������� ���ҵ˴ϴ�.
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
