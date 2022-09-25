using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private int id;
    public int ID { get { return id;} }

    [SerializeField]
    private ItemType type;
    public ItemType Type { get { return type; } }

    [SerializeField]
    private EquipHandType equipHandType;
    public EquipHandType EquipHandType { get { return equipHandType; } }

    [SerializeField]
    private string itemName;
    public string Name { get { return itemName; } }  

    [SerializeField]
    private string description;
    public string Description { get { return description; } }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    [SerializeField]
    private GameObject worldObject;
    public GameObject WorldObject { get { return worldObject; } }

    [SerializeField]
    private StatusInfo statusInfoData;
    public StatusInfo StatusInfoData { get { return statusInfoData; } }

    [SerializeField]
    private StatusInfo addStatusInfoData;
    public StatusInfo AddStatusInfoData { get { return addStatusInfoData; } }
    
    [SerializeField]
    private StatusInfo subStatusInfoData;
    public StatusInfo SubStatusInfoData { get { return subStatusInfoData; } }

    [Button("AutoGenerate")]
    public void AutoGenerate()
    {
        statusInfoData.Clear();
        addStatusInfoData.Clear();
        subStatusInfoData.Clear();
    }
}
