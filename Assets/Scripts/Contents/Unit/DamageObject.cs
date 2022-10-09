using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour, IDamageable
{
    [SerializeField]
    private UnitStatus status;

    public bool OnDamage(DamageInfo damageInfo)
    {
        return status.OnDamage(damageInfo.damage);
    }
}
