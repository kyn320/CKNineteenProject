using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGolemHitState : MonsterHitState
{
    public override void DamageHit(DamageInfo damageInfo)
    {
        if (!isStay)
            return;

        base.DamageHit(damageInfo);
        Instantiate(vfxPrefabData.GetVFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
    }
}
