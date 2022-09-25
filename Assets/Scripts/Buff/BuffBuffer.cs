using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBuffer : MonoBehaviour
{
    public List<BuffBase> buffBuffer = new List<BuffBase>();
    public List<BuffBase> deBuffBuffer = new List<BuffBase>();

    public void AddBuffer(BuffBase buff)
    {
        buffBuffer.Add(buff);
    }

    public void AddDeBuffer(BuffBase buff)
    {
        deBuffBuffer.Add(buff);
    }

    public void Update()
    {
        foreach(var buff in buffBuffer)
        {
            if(!buff.GetActiveBuff())
                buffBuffer.Remove(buff);
        }

        foreach (var buff in deBuffBuffer)
        {
            if (!buff.GetActiveBuff())
                deBuffBuffer.Remove(buff);
        }
    }
}
