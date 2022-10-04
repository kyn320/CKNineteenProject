using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MonsterStateType
{
    MONSTERSTATE_IDLE = 0,
    MONSTERSTATE_CHASE,
    MONSTERSTATE_ATTACK,
    MONSTERSTATE_HIT,
    MONSTERSTATE_DEATH
};