using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PotionAreaEvent : MonoBehaviour
{
    [SerializeField]
    WeaponData weaponData;
    [SerializeField]
    PlayerStatus playerStatus;
    [SerializeField]
    ProjectileController projectileController;
    [SerializeField]
    string[] userTags;
    [SerializeField]
    float intervalDelayTime = 2f;
    float intervalDelayTimer = 2f;

    public UnityEvent<GameObject> IntervalEvents;

    [SerializeField]
    GameObject hitEffect;
    GameObject hitEffectObj;

    [SerializeField]
    BuffData buffdata;

    [SerializeField]
    private DamageInfo damageInfo;
    [SerializeField]
    VFXPrefabData vfxPrefabData;
    [SerializeField]
    SFXPrefabData sfxPrefabData;


    [SerializeField]
    private float lavaDelayTime = 1f;
    [SerializeField]
    private float lavaDelayTimer = 0f;

    public UnityEvent<bool> hitEvnet;

    float lifeTime;

    private void Start()
    {
        lifeTime = GetComponent<AutoDestroyByLifetime>().lifeTime - 0.5f;
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        lavaDelayTimer = lavaDelayTime;
    }

    public void SetWeaponData(WeaponData weaponData)
    {
        this.weaponData = weaponData;
    }

    public void UserSearch(Collider collider)
    {
        for (int i = 0; i < userTags.Length; i++)
            if (collider.gameObject.tag == userTags[i])
            {

                GameObject user = collider.gameObject;

                //PlayerStatus는 player밖에 없음으로 문제가 될 수 있음 만약 문제가 된다면 수정해주세요
                playerStatus = user.GetComponent<PlayerStatus>();

                if (hitEffect)
                {
                    hitEffectObj = Instantiate(hitEffect, user.transform);
                }


                if (collider.gameObject.CompareTag("Player"))
                    return;

                var damageable = collider.gameObject.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageInfo.hitPoint = collider.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    damageInfo.hitNormal = (transform.position - collider.transform.position).normalized;

                    var resultDamageInfo = damageable.OnDamage(damageInfo);

                    if (resultDamageInfo.isHit)
                    {
                        if (resultDamageInfo.isCritical)
                        {
                            Instantiate(vfxPrefabData.GetVFXPrefab("CriticalHit"), damageInfo.hitPoint, Quaternion.identity);
                            Instantiate(sfxPrefabData.GetSFXPrefab("CriticalHit"), damageInfo.hitPoint, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(vfxPrefabData.GetVFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
                            Instantiate(sfxPrefabData.GetSFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
                        }

                        hitEvnet?.Invoke(resultDamageInfo.isKill);
                    }
                }
            }
    }
    public void LavaBurst(Collider collider)
    {
        GameObject user = collider.gameObject;
        Debug.Log(user.GetComponent<Rigidbody>().velocity);
        user.GetComponent<Rigidbody>().velocity = (Vector3.up * 10f);
        //Debug.Log(user.GetComponent<Rigidbody>().velocity);

        float damageResult = weaponData.StatusInfoData.GetElement(StatusType.MaxAttackPower).GetAmount();


        lavaDelayTimer += Time.deltaTime;

        if (lavaDelayTime < lavaDelayTimer)
        {
            lavaDelayTimer = 0f;

            user.GetComponent<MonsterController>()?.OnDamage(new DamageInfo()
            {
                hitPoint = user.transform.position,
                damage = damageResult,
                isCritical = false,
                isKnockBack = false
            }
         );
        }

        
    }

    public void IntervalEvent(Collider collider)
    {
        GameObject user = collider.gameObject;
        if (user)
        {
            intervalDelayTimer += Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
                Destroy(hitEffectObj);


            if (intervalDelayTimer > intervalDelayTime)
            {
                intervalDelayTimer = 0;
                IntervalEvents?.Invoke(user);
            }
        }
    }

    public void Healing(Collider collider)
    {
        GameObject user = collider.gameObject;

        float healPoint = 1f/*weaponData.StatusInfoData.GetElement(StatusType.MinAttackPower).GetAmount()*/;
        intervalDelayTimer += Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(hitEffectObj);


        if (intervalDelayTimer > intervalDelayTime)
        {
            intervalDelayTimer = 0;
            playerStatus.AddHP(playerStatus.currentStatus.GetElement(StatusType.MinAttackPower).GetAmount(), true);
            user.GetComponent<PlayerStatus>()?.AddHP(healPoint, true);
        }

    }

    public void deBuff(Collider collider)
    {
        GameObject user = collider.gameObject;
        if (collider.gameObject.transform.root)
            user = collider.gameObject.transform.root.gameObject;

        Debug.Log($"user : {user.name}");

        if (user)
        {
            BuffController buffController = user.GetComponent<BuffController>();
            int findBuffNum = -1;
            for (int i = 0; buffController.crowdBehaviourList.Count > i; i++)
            {
                if (buffController.crowdBehaviourList[i])
                    if (buffController.crowdBehaviourList[i].name == buffdata.BuffBehaviourObject.name + "(Clone)")
                        findBuffNum = i;
            }

            if (findBuffNum != -1)
                buffController.crowdBehaviourList[findBuffNum].ResetCooltime();
            else
                buffController.AddCrwod(buffdata);
        }
    }

    public void UserExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (hitEffect)
            {
                Destroy(hitEffectObj);
            }
        }
    }
    public void SetCalculate(ProjectileController controller)
    {
        damageInfo.isCritical = controller.GetCritical();
        damageInfo.damage = controller.CalculateDamageAmount(damageInfo.isCritical);
    }
}
