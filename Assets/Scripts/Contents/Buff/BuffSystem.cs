using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    private StatusInfo status = null;

    private void Awake()
    {
        status = GetComponent<UnitStatus>().currentStatus;

        CreateBuffer(BuffObjectType.BUFFOBJECTTYPE_PLAYER);
    }

    public void AddBuff(StatusType type, float time, float count) 
    {

    }

    public void AddDeBuff(StatusType type, float time, float count)
    {

    }

    public void AddCrowd()
    {

    }

    private void CreateBuffer(BuffObjectType type) {
        //BuffController.Instance.CreateBuffer();
    }
}
