using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTestPlayer : MonoBehaviour
{
    public StatusInfoData status;
    public StatusInfoData buffData;

    public void Awake()
    {
        BuffBuffer.Instance.CreateBuffer(StatusType.HP, 3f, 10, status.StausDic[StatusType.HP].GetAmount());
    }
    public void Update()
    {
    }
}
