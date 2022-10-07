using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBuffer : MonoBehaviour
{
    public float timer = .0f;
    public List<BuffBase> buffBase = null;

    public BuffBuffer(float timer, BuffBase buffBase)
    {
        this.timer = timer;
        this.buffBase.Add(buffBase);
    }

    public bool ReturnEffectTimer()
    {
        for (var i = 0; i < buffBase.Count; i++)
        {
            if (timer >= buffBase[i].effectTime)
                return true;
        }
        return false;
    }
}
