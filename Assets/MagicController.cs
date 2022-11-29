using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicController : MonoBehaviour
{
    [SerializeField]
    WeaponData weaponData;
    [SerializeField]
    PlayerStatus playerStatus;
    [SerializeField]
    ProjectileController projectileController;
    [SerializeField]
    string[] userTags;

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


    private void Start()
    {
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
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

    public void deBuff(Collider collider)
    {
        GameObject user = collider.gameObject;
        if (collider.gameObject.transform.root)
            user = collider.gameObject.transform.root.gameObject;

        Debug.Log($"user : {user.name}");

        if (user.CompareTag("Monster"))
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
