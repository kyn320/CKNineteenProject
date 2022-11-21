using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "SFXPrefabData", menuName = "SFX/SFXPrefabData", order = 0)]
public class SFXPrefabData : ScriptableObject
{
    [SerializeField]
    private SerializableDictionary<string, GameObject> sfxDic = new SerializableDictionary<string, GameObject>();

    public SerializableDictionary<string, GameObject> SfxDic { get { return sfxDic; } }

    public GameObject GetSFXPrefab(string name)
    {
        if (sfxDic.ContainsKey(name))
            return sfxDic[name];

        return null;
    }

}
