using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : Singleton<BuffController>
{
    Dictionary<BuffObjectType, BuffBuffer> buffer = new Dictionary<BuffObjectType, BuffBuffer>();


    public void AddBuffer()
    {

    }

    public void AddDebuffer()
    {

    }

    public void CreateBuffer(BuffObjectType type, float time, float count)
    {
    }
}
