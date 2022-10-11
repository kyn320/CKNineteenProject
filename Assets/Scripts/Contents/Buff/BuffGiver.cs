using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffGiver : MonoBehaviour
{
    public List<BuffData> giveBuffList;

    private void OnTriggerEnter(Collider other)
    {
        var buffController = other.GetComponent<BuffController>();
        if (buffController == null)
            return;

        for (var i = 0; i < giveBuffList.Count; ++i)
        {
            buffController.AddBuff(giveBuffList[i]);
        }
    }

}
