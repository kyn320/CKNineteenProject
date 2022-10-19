using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour, IDamageable
{
    [SerializeField]
    private UnitStatus status;

    public List<string> allowDamageTags;

    public DamageInfo OnDamage(DamageInfo damageInfo)
    {
        if(allowDamageTags.Count > 0) {
            if (!string.IsNullOrEmpty(damageInfo.ownerTag) && !allowDamageTags.Contains(damageInfo.ownerTag))
            {
                damageInfo.isHit = false;
                damageInfo.isKill = false;
                return damageInfo;
            }
        }

        return status.OnDamage(damageInfo);
    }
}
