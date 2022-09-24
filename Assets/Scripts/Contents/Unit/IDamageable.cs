using UnityEngine;

public interface IDamageable {
    bool OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}