using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "KeySettingData", menuName = "Setting/Key", order = 0)]
public class KeySettingData : ScriptableObject
{
    [SerializeField]
    public SerializableDictionary<KeyType, KeyBind> keyBindDic = new SerializableDictionary<KeyType, KeyBind>();


    [Button("AutoGenerate")]
    public void AutoGenerate()
    {
        keyBindDic.Clear();

        IEnumerable<KeyType> keyTypeList =
                Enum.GetValues(typeof(KeyType)).Cast<KeyType>();

        foreach (KeyType keyType in keyTypeList) {
            keyBindDic.Add(keyType, new KeyBind() { name = keyType.ToString(), type = keyType });
        }
    }

    public KeyBind GetKeyBind(KeyType keyType)
    {
        return keyBindDic[keyType];
    }

}
