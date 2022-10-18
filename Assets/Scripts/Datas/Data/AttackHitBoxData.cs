using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "AttackHitBoxData", menuName = "HitBox/AttackHitBoxData", order = 0)]
public class AttackHitBoxData : ScriptableObject
{
    [SerializeField]
    private Vector3 createPosition;
    public Vector3 CreatePosition { get { return createPosition; } }

    [SerializeField]
    private GameObject hitBoxPrefab;
    public GameObject HitBoxPrefab { get { return hitBoxPrefab; } }
}
