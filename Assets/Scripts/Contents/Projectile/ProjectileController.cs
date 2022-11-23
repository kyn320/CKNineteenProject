using System;
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
    private bool isDontDestroy = false;

    [SerializeField]
    protected bool isMove = false;

    [SerializeField]
    protected VFXPrefabData vfxPrefabData;

    private Func<bool> calculateCritical;
    private Func<bool, float> calculateDamage;

    public void SetCalculator(Func<bool> calculateCritical, Func<bool, float> calculateDamage)
    {
        this.calculateCritical = calculateCritical;
        this.calculateDamage = calculateDamage;
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

    public void Hit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            return;

        var damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageInfo.isCritical = calculateCritical();
            damageInfo.damage = calculateDamage(damageInfo.isCritical);
            damageInfo.hitPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            damageInfo.hitNormal = (transform.position - other.transform.position).normalized;

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

        if(!isDontDestroy /*|| collision.gameObject.CompareTag("Ground")*/)
        gameObject.SetActive(false);
    }

}
