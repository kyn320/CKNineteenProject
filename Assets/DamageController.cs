using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField]
    private StatusInfoData currentStatus;
    
    private UnitStatus target;

    private bool isActive = false;
    public bool isOnKnockback;

    public bool CheckHasUnitStatus(GameObject unit)
    {
        return unit.GetComponent<UnitStatus>() != null ? true : false;
    }

    public void SetDamageToUnit(GameObject unit)
    {
        if (!CheckHasUnitStatus(unit))
            return;

        isActive = true;

        target = unit.GetComponent<UnitStatus>();

        float damageResult = currentStatus.StausDic[StatusType.MaxAttackPower].GetAmount();

        target?.OnDamage(new DamageInfo()
        {
            damage = damageResult,
            isKnockBack = isOnKnockback
        });
    }

    private void ObjectEnter(GameObject unit)
    {
        SetDamageToUnit(unit);
    }

    private void ObjectExit(GameObject unit)
    {
        Debug.Log(target.currentStatus.GetElement(StatusType.HP).GetAmount());

        target = null;
        isActive = false;

    }

    public void OnCollisionEnter(Collision collision)
    {
        if(isActive && target != null)
            return;

        ObjectEnter(collision.gameObject);
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

        ObjectEnter(other.gameObject);
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
