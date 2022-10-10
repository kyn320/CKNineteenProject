using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct DamageInfo
{
    public GameObject owner;
    public string ownerTag;

    public bool isHit;
    public bool isCritical;
    public bool isKnockBack;
    public bool isKill;
    public float damage;

    public Vector3 hitPoint;
    public Vector3 hitNormal;
}