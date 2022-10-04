using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAttackPatternRangeGroup
{
    [SerializeField]
    private float percent;

    public float Percent { get { return percent; } }

    [SerializeField]
    private MonsterAttackPattern attackPattern;

    public MonsterAttackPattern AttackPattern { get { return attackPattern; } }

}
