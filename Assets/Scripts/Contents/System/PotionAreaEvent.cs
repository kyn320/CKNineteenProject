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
    float intervalDelayTimer;

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

    public UnityEvent<bool> hitEvnet;

    float lifeTime;

    private void Start()
    {
        lifeTime = GetComponent<AutoDestroyByLifetime>().lifeTime - 0.5f;
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    public void SetWeaponData(WeaponData weaponData)
    {
        this.weaponData = weaponData;
    }

    public void UserSearch(Collider collider)
    {   
        for(int i = 0; i < userTags.Length; i++)
        if (collider.gameObject.tag == userTags[i])
        {
            if (collider.gameObject.CompareTag("Player"))
                return;

            GameObject user = collider.gameObject;

            //PlayerStatus�� player�ۿ� �������� ������ �� �� ���� ���� ������ �ȴٸ� �������ּ���
            playerStatus = user.GetComponent<PlayerStatus>();

            if (hitEffect)
            {
                hitEffectObj = Instantiate(hitEffect, user.transform);
            }

            var damageable = collider.gameObject.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageInfo.isCritical = projectileController.GetCritical();
                    damageInfo.damage = projectileController.GetDamage();
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
        Debug.Log(user.GetComponent<Rigidbody>().velocity);

        float damageResult = weaponData.StatusInfoData.GetElement(StatusType.MaxAttackPower).GetAmount();

        user.GetComponent<MonsterController>()?.OnDamage(new DamageInfo()
        {
            damage = damageResult,
            isCritical = false,
            isKnockBack = false
        });
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
        if (user.CompareTag("Player"))
        {
            //healPoint�� �ӽ� ���� �� ������ �� ��
            float healPoint = 1f;
            intervalDelayTimer += Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
                Destroy(hitEffectObj);


            if (intervalDelayTimer > intervalDelayTime)
            {
                intervalDelayTimer = 0;
                playerStatus.AddHP(playerStatus.currentStatus.GetElement(StatusType.MinAttackPower).GetAmount(), true);
                user.GetComponent<PlayerStatus>().AddHP(healPoint, true);
            }
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
                if(buffController.crowdBehaviourList[i])
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
}
