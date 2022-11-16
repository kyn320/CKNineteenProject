using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDamageText : MonoBehaviour
{
    [SerializeField]
    private TargetFollower targetFollower;

    [SerializeField]
    private UIAmountText amountText;

    public void SetDamageAmount(Transform target, float damage)
    {
        targetFollower.target = target;
        amountText.UpdateAmount(damage);
    }



}
