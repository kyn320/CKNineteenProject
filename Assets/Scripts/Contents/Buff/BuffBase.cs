using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Buff/BuffBase", order = 0)]

public class BuffBase : SerializedScriptableObject
{
    public StatusType statusType;
    public float effectTime;
    public float abilityCount;

    public BuffBase(StatusType type, float time, float count)
    {
        statusType = type;
        effectTime = time;
        abilityCount = count;
    }
}
