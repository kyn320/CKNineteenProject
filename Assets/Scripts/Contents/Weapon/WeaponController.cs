using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private GameObject ownerObject;

    [SerializeField]
    private WeaponData weaponData;

    [SerializeField]
    private DamageInfo damageInfo;
    public UnityEvent<bool> hitEvnet;

    [SerializeField]
    protected VFXPrefabData vfxPrefabData;
    [SerializeField]
    protected SFXPrefabData sfxPrefabData;

    public float hitPauseWaitTime = 0.1f;
    public float hitPauseTime = 0.1f;

    private Func<bool> calculateCritical;
    private Func<bool, float> calculateDamage;


    public void SetOwnerObject(GameObject ownerObject)
    {
        this.ownerObject = ownerObject;
    }

    public void SetCalculator(Func<bool> calculateCritical, Func<bool, float> calculateDamage)
    {
        this.calculateCritical = calculateCritical;
        this.calculateDamage = calculateDamage;
    }

    public void SetWeaponData(WeaponData weaponData)
    {
        this.weaponData = weaponData;
    }

    public void CreateAttackHitBox(int index)
    {
        var hitBoxData = weaponData.HitBoxDataList[0];
        var hitBoxObject = Instantiate(hitBoxData.HitBoxPrefab);

        hitBoxObject.transform.forward = ownerObject.transform.forward;
        var hitBoxPosition = ownerObject.transform.position;

        hitBoxPosition += ownerObject.transform.right * hitBoxData.CreatePosition.x;
        hitBoxPosition.y += hitBoxData.CreatePosition.y;
        hitBoxPosition += ownerObject.transform.forward * hitBoxData.CreatePosition.z;

        hitBoxObject.transform.position = hitBoxPosition;
        hitBoxObject.GetComponent<AttackHitBox>().triggerEnterEvent.AddListener(Hit);
    }


    public void Hit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            return;

        var damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageInfo.isCritical = calculateCritical();
            damageInfo.damage = calculateDamage(damageInfo.isCritical);
            damageInfo.hitPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            damageInfo.hitNormal = (transform.position - other.transform.position).normalized;

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


        var hitPause = other.gameObject.GetComponent<IHitPauseable>();
        hitPause.HitPause(hitPauseWaitTime, hitPauseTime);

        gameObject.SetActive(false);
    }

}
