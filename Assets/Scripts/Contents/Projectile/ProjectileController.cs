using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private ProjectileMoveType moveType;

    [SerializeField]
    private ProjectileMoveable moveable;

    public DamageInfo damageInfo;

    [SerializeField]
    protected bool isMove = false;

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
            damageable.OnDamage(damageInfo, contact.point, contact.normal);
        }
    }

}
