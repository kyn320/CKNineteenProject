using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private ProjectileMoveType moveType;

    [SerializeField]
    private ProjectileMoveable moveable;

    [SerializeField]
    private DamageInfo damageInfo;

    public UnityEvent<bool> hitEvnet;

    [SerializeField]
    protected bool isMove = false;

    [SerializeField]
    protected VFXPrefabData vfxPrefabData;

    public void SetStatus(float attackPower, bool isCritical)
    {
        damageInfo.damage = attackPower;
        damageInfo.isCritical = isCritical;
    }

    public void Shot(Vector3 startPoint, Vector3 moveDirection, float moveSpeed, float maxDistance)
    {
        moveable.SetStartPoint(startPoint);
        moveable.SetDirection(moveDirection);
        moveable.SetMoveSpeed(moveSpeed);
        moveable.SetMaxDistance(maxDistance);
        isMove = true;
    }

    public void FixedUpdate()
    {
        if (isMove)
            moveable.Move();
    }

    public void Hit(Collision collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            var contact = collision.contacts[0];
            damageInfo.hitPoint = contact.point;
            damageInfo.hitNormal = contact.normal;

            var isKill = damageable.OnDamage(damageInfo);
            if (damageInfo.isCritical)
            {
                Instantiate(vfxPrefabData.GetVFXPrefab("CriticalHit"), damageInfo.hitPoint, Quaternion.identity);
            }
            else
            {
                Instantiate(vfxPrefabData.GetVFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
            }
            hitEvnet?.Invoke(isKill);
        }

        gameObject.SetActive(false);
    }

}
