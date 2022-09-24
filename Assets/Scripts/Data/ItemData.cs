using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private int id;
    public int ID { get { return id;} }

    [SerializeField]
    private string saveFileName = "SaveFile";
    public string SaveFileName { get { return saveFileName; } }

}
