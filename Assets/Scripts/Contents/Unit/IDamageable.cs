using UnityEngine;

public interface IDamageable {
    bool OnDamage(DamageInfo damageInfo, Vector3 hitPoint, Vector3 hitNormal);
}