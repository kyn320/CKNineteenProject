using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase
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
