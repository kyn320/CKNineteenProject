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

    public float damageResult = 1f;


    private void Start()
    {
        //�ӽÿ�, �Ҿ����ϰ� ��ȿ�����̱� ������ �ݵ�� �ٲܰ�!
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
