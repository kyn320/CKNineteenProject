using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemCriticalHitState : MonsterHitState
{
    public override void DamageHit(DamageInfo damageInfo)
    {
        if (!isStay)
            return;

        base.DamageHit(damageInfo);
        KnockBack();
        Instantiate(vfxPrefabData.GetVFXPrefab("CriticalHit"), damageInfo.hitPoint, Quaternion.identity);
    }

}
