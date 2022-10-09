using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Item/WeaponData", order = 0)]
public class WeaponData : ItemData
{
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType { get { return weaponType; } }

    [SerializeField]
    private ProjectileMoveType projectileMoveType;
    public ProjectileMoveType ProjectileMoveType { get { return projectileMoveType; } }

    [SerializeField]
    private int attackAnimationType;
    public int AttackAnimationType { get { return attackAnimationType; } }

    [SerializeField]
    private Vector3 spawnVector;
    public Vector3 SpawnVector { get { return spawnVector; } }

    [SerializeField]
    private float spawnTime;
    public float SpawnTime { get { return spawnTime; } }

}
