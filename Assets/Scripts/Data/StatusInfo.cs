using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[System.Serializable]
public class StatusInfo
{
    [SerializeField]
    protected SerializableDictionary<StatusType, StatusElement> statusDic = new SerializableDictionary<StatusType, StatusElement>();

    public void Copy(Dictionary<StatusType, StatusElement> copyDic)
    {
        statusDic.Clear();

        var keyList = copyDic.Keys.ToList();
        for (var i = 0; i < keyList.Count; ++i)
        {
            statusDic.Add(keyList[i], new StatusElement(copyDic[keyList[i]]));
        }
    }

    public StatusElement GetElement(StatusType statusType) {
        return statusDic[statusType];
    }
}
