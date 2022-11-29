using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;

[System.Serializable]
public class StatusInfo
{
    [SerializeField]
    protected SerializableDictionary<StatusType, StatusElement> statusDic = new SerializableDictionary<StatusType, StatusElement>();

    public void Copy(StatusInfo status)
    {
        statusDic.Clear();
        var copyDic = status.statusDic;
        var keyList = copyDic.Keys.ToList();

        for (var i = 0; i < keyList.Count; ++i)
        {
            statusDic.Add(keyList[i], new StatusElement(copyDic[keyList[i]]));
        }
    }

    public void Copy(Dictionary<StatusType, StatusElement> copyDic)
    {
        statusDic.Clear();

        var keyList = copyDic.Keys.ToList();
        for (var i = 0; i < keyList.Count; ++i)
        {
            statusDic.Add(keyList[i], new StatusElement(copyDic[keyList[i]]));
        }
    }

    public bool ContainsElement(StatusType statusType)
    {
        return statusDic.ContainsKey(statusType);
    }

    public StatusElement GetElement(StatusType statusType)
    {
        return statusDic[statusType];
    }

    public void AddStatusInfo(StatusInfo statusInfo)
    {
        foreach (StatusType statusType in statusDic.Keys)
        {
            if (statusType == StatusType.None || !statusInfo.ContainsElement(statusType))
                continue;

            var targetElement = GetElement(statusType);
            var addElement = statusInfo.GetElement(statusType);

            Debug.Log(addElement.type + " / " + addElement.GetAmount());

            targetElement.AddAmount(addElement.GetAmount());
            targetElement.AddPercent(addElement.GetPercent());

            Debug.Log(targetElement.type + " / " + targetElement.GetAmount());
        }
    }

    public void SubStatusInfo(StatusInfo statusInfo)
    {
        foreach (StatusType statusType in statusDic.Keys)
        {
            if (statusType == StatusType.None || !statusInfo.ContainsElement(statusType))
                continue;

            var targetElement = GetElement(statusType);
            var subElement = statusInfo.GetElement(statusType);


            targetElement.SubAmount(subElement.GetAmount());
            targetElement.SubPercent(subElement.GetPercent());
        }
    }

    public void Clear()
    {
        statusDic.Clear();

        IEnumerable<StatusType> StatusTypeList =
                Enum.GetValues(typeof(StatusType)).Cast<StatusType>();

        foreach (StatusType statusType in StatusTypeList)
        {
            if (statusType == StatusType.None)
                continue;

            statusDic.Add(statusType, new StatusElement() { name = statusType.ToString(), type = statusType });
        }
    }
}
