using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "UIColorData", menuName = "UI/UIColorData", order = 0)]
public class UIColorData : SerializedScriptableObject
{

    [SerializeField]
    public SerializableDictionary<string, Color> colorDic = new SerializableDictionary<string, Color>();


}
