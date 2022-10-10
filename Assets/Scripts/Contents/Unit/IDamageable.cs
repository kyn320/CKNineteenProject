using UnityEngine;

public interface IDamageable {
    DamageInfo OnDamage(DamageInfo damageInfo);
}