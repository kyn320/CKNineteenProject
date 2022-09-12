using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "StatusInfoData", menuName = "Status/StatusInfo", order = 0)]
public class StatusInfoData : SerializedScriptableObject
{
    [SerializeField]
    private SerializableDictionary<StatusType, StatusElement> statusDic = new SerializableDictionary<StatusType, StatusElement>();

    public SerializableDictionary<StatusType, StatusElement> StausDic { get { return statusDic; } }

    [Button("AutoGenerate")]
    public void AutoGenerate()
    {
        statusDic.Clear();

        IEnumerable<StatusType> StatusTypeList =
                Enum.GetValues(typeof(StatusType)).Cast<StatusType>();

        foreach (StatusType statusType in StatusTypeList)
        {
            if(statusType == StatusType.None)
                continue;

            statusDic.Add(statusType, new StatusElement() { name = statusType.ToString(), type = statusType });
        }
    }
}
