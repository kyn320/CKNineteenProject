using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField]
    private StatusInfoData currentStatus;

    private IDamageable target;

    private bool isActive = false;
    public bool isOnKnockback;

    public VFXPrefabData vfxPrefabData;

    public bool CheckHasUnitStatus(GameObject unit)
    {
        return unit.GetComponent<UnitStatus>() != null ? true : false;
    }

    public void SetDamageToUnit(GameObject unit, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!CheckHasUnitStatus(unit))
            return;

        isActive = true;

        target = unit.GetComponent<IDamageable>();

        float damageResult = currentStatus.StausDic[StatusType.MaxAttackPower].GetAmount();

        var hitObject = Instantiate(vfxPrefabData.GetVFXPrefab("Hit"), hitPoint, Quaternion.identity);

        target?.OnDamage(new DamageInfo()
        {
            damage = damageResult,
            hitPoint = hitPoint,
            hitNormal = hitNormal,
            isKnockBack = isOnKnockback
        });
    }

    private void ObjectEnter(GameObject unit, Vector3 hitPoint, Vector3 hitNormal)
    {
        SetDamageToUnit(unit, hitPoint, hitNormal);
    }

    private void ObjectExit(GameObject unit)
    {
        target = null;
        isActive = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (isActive && target != null)
            return;

        var contact = collision.contacts[0];

        ObjectEnter(collision.gameObject, contact.point, contact.normal);
    }

    public void OnCollisionExit(Collision collision)
    {
        if (isActive && target != null)
        {
            ObjectExit(collision.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isActive && target != null)
            return;

        ObjectEnter(other.gameObject, transform.position, (transform.position - other.transform.position).normalized);
    }

    public void OnTriggerExit(Collider other)
    {
        if (isActive && target != null)
        {
            Debug.Log("INNER EXIT");
            ObjectExit(other.gameObject);
        }
    }
}
