using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemHitState : MonsterHitState
{
    public override void DamageHit(DamageInfo damageInfo)
    {
        if(!isStay)
            return;

        base.DamageHit(damageInfo);
        KnockBack();
        Instantiate(vfxPrefabData.GetVFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
    }

}
