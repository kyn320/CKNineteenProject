using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoadPathData", menuName = "System/SaveLoadPathData", order = 0)]
public class SaveLoadPathData : ScriptableObject
{
    [SerializeField]
    private string saveFileName = "SaveFile";
    public string SaveFileName { get { return saveFileName; } }

}
