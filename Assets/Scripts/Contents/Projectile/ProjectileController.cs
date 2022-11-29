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
    [SerializeField]
    protected SFXPrefabData sfxPrefabData;

    private StatusInfo status = new StatusInfo();
    protected StatusCalculator damageCalculator;
    protected StatusCalculator criticalDamageCalculator;

    public void SetStatus(StatusInfo calculateStatus)
    {
        status.Copy(calculateStatus);
    }

    public void SetOwnerObject(GameObject ownerObject)
    {
        damageInfo.owner = ownerObject;
        damageInfo.ownerTag = ownerObject.tag;
    }

    public void SetCalculator(StatusCalculator damageCalculator, StatusCalculator criticalDamageCalculator)
    {
        this.damageCalculator = damageCalculator;
        this.criticalDamageCalculator = criticalDamageCalculator;
    }

    public void Shot(Vector3 startPoint, Vector3 endPoint, Vector3 moveDirection, float moveSpeed, float maxDistance)
    {
        moveable.SetStartPoint(startPoint);
        moveable.SetEndPoint(endPoint);
        moveable.SetDirection(moveDirection);
        moveable.SetMoveSpeed(moveSpeed);
        moveable.SetMaxDistance(maxDistance);
        moveable.Shot();

        Instantiate(sfxPrefabData.GetSFXPrefab("Shot"), startPoint, Quaternion.identity);
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
            damageInfo.isCritical = GetCritical();
            damageInfo.damage = CalculateDamageAmount(damageInfo.isCritical);
            damageInfo.hitPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            damageInfo.hitNormal = (transform.position - other.transform.position).normalized;

            var resultDamageInfo = damageable.OnDamage(damageInfo);

            if (resultDamageInfo.isHit)
            {
                if (resultDamageInfo.isCritical)
                {
                    Instantiate(vfxPrefabData.GetVFXPrefab("CriticalHit"), damageInfo.hitPoint, Quaternion.identity);
                    Instantiate(sfxPrefabData.GetSFXPrefab("CriticalHit"), damageInfo.hitPoint, Quaternion.identity);
                }
                else
                {
                    Instantiate(vfxPrefabData.GetVFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
                    Instantiate(sfxPrefabData.GetSFXPrefab("Hit"), damageInfo.hitPoint, Quaternion.identity);
                }

                hitEvnet?.Invoke(resultDamageInfo.isKill);
            }
        }

        if (!isDontDestroy /*|| collision.gameObject.CompareTag("Ground")*/)
            gameObject.SetActive(false);
    }

    public bool GetCritical()
    {
        var tryPercent = UnityEngine.Random.Range(0f, 100f);
        return tryPercent <= status.GetElement(StatusType.CriticalPercent).CalculateTotalAmount();
    }

    public float CalculateDamageAmount(bool isCritical)
    {
        if (isCritical)
        {
            return criticalDamageCalculator.Calculate(status);
        }
        else
        {
            return damageCalculator.Calculate(status);
        }
    }
}
