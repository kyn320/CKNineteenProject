using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerStateType
{
    Idle,
    Move,
    Jump,
    Air,
    Attack,
    Hit,
    CriticalHit,
    Death,
}
