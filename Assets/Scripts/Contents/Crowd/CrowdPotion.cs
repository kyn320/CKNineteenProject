using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPotion : CrowdBehaviour
{
    /// <summary>
    /// 일정 시간마다 체력의 %비율이 지속적으로 감소됩니다.
    /// </summary>
    /// 

    [SerializeField]
    private float activeCalcTime = .0f;
    [SerializeField]
    private float activeStandardTime = .0f;

    public float damageResult = 1f;


    private void Start()
    {
        //임시용, 불안정하고 비효율적이기 때문에 반드시 바꿀것!
        damageResult = GameObject.Find("Player").GetComponent<PlayerStatus>().currentStatus.GetElement(StatusType.MinAttackPower).GetAmount();
    }

    public override void Active()
    {
        activeCalcTime -= Time.deltaTime;

        if (activeCalcTime <= 0)
        {
            string userTag = transform.parent.tag;


            if(userTag == "Monster")
            {


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
