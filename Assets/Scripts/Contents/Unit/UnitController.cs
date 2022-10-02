using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnitStatus))]
public class UnitController : MonoBehaviour, IDamageable
{
    private UnitStatus status;

    protected virtual void Awake() {
        status = GetComponent<UnitStatus>();    
    }

    public virtual bool OnDamage(DamageInfo damageInfo, Vector3 hitPoint, Vector3 hitNormal)
    {
        return status.OnDamage(damageInfo.damage);
    }
}
