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

    public void SetOwnerObject(GameObject ownerObject)
    {
        this.ownerObject = ownerObject;
    }

    public void SetWeaponData(WeaponData weaponData)
    {
        this.weaponData = weaponData;
    }

    public void SetStatus(float attackPower, bool isCritical)
    {
        damageInfo.damage = attackPower;
        damageInfo.isCritical = isCritical;
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
        hitBoxObject.GetComponent<AttackHitBox>().collisionEnterEvent.AddListener(Hit);
    }


    public void Hit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        var damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            var contact = collision.contacts[0];
            damageInfo.hitPoint = contact.point;
            damageInfo.hitNormal = contact.normal;

            var resultDamageInfo = damageable.OnDamage(damageInfo);

            if (resultDamageInfo.isHit)
            {
                if (resultDamageInfo.isCritical)
                {
                    Instantiate(vfxPrefabData.GetVFXPrefab("CriticalHit"), damageInfo.hitPoint, Quaternion.identity);
                }
                else
                {
                    Instantiate(vfxPrefabData.GetVFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
                }

                hitEvnet?.Invoke(resultDamageInfo.isKill);
            }
        }

        gameObject.SetActive(false);
    }

}
