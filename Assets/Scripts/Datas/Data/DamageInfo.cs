using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct DamageInfo
{
    public bool isCritical;
    public bool isKnockBack;
    public float damage;

    public Vector3 hitPoint;
    public Vector3 hitNormal;
}