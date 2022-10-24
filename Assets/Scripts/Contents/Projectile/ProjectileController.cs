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

    public UnityEvent shotEvnet;
    public UnityEvent<bool> hitEvnet;

    [SerializeField]
    private bool isPiercing = false;

    [SerializeField]
    protected bool isMove = false;

    [SerializeField]
    protected VFXPrefabData vfxPrefabData;

    public void SetStatus(float attackPower, bool isCritical)
    {
        damageInfo.damage = attackPower;
        damageInfo.isCritical = isCritical;
    }

    public void Shot(Vector3 startPoint,Vector3 endPoint,  Vector3 moveDirection, float moveSpeed, float maxDistance)
    {
        moveable.SetStartPoint(startPoint);
        moveable.SetEndPoint(endPoint);
        moveable.SetDirection(moveDirection);
        moveable.SetMoveSpeed(moveSpeed);
        moveable.SetMaxDistance(maxDistance);
        moveable.Shot();
        isMove = true;
        shotEvnet?.Invoke();
    }

    public void FixedUpdate()
    {
        if (isMove)
            moveable.Move();
    }

    public void Hit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        var damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            var contact = collision.contacts[0];
            damageInfo.hitPoint = contact.point;
            damageInfo.hitNormal = contact.normal;

            var resultDamageInfo = damageable.OnDamage(damageInfo);

            if (resultDamageInfo.isHit)
            {
                if (resultDamageInfo.isCritical)
                {
                    Instantiate(vfxPrefabData.GetVFXPrefab("CriticalHit"), damageInfo.hitPoint, Quaternion.identity);
                }
                else
                {
                    Instantiate(vfxPrefabData.GetVFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
                }

                hitEvnet?.Invoke(resultDamageInfo.isKill);
            }
        }

        if(!isPiercing /*|| collision.gameObject.CompareTag("Ground")*/)
        gameObject.SetActive(false);
    }

}
