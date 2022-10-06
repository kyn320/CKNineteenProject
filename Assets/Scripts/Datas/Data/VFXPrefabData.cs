using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "VFXPrefabData", menuName = "VFX/VFXPrefabData", order = 0)]
public class VFXPrefabData : ScriptableObject
{
    [SerializeField]
    private SerializableDictionary<string, GameObject> vfxDic = new SerializableDictionary<string, GameObject>();

    public SerializableDictionary<string, GameObject> VfxDic { get { return vfxDic; } }

    public GameObject GetVFXPrefab(string name)
    {
        if (vfxDic.ContainsKey(name))
            return vfxDic[name];

        return null;
    }

}
