using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "ItemDataContainer", menuName = "DataContainer/ItemDataContainer", order = 0)]
public class ItemDataContainer : ScriptableObject
{
    [SerializeField]
    private List<ItemData> items;

    public List<ItemData> Items { get { return items; } }

    [SerializeField]
    private SerializableDictionary<int, ItemData> itemDic = new SerializableDictionary<int, ItemData>();



    [Button("자동 데이터 캐싱")]
    public void AutoGenerate()
    {
        itemDic.Clear();
        for (var i = 0; i < items.Count; ++i)
        {
            itemDic.Add(items[i].ID, items[i]);
        }
    }

    public ItemData FindItem(int id)
    {
        if (itemDic.ContainsKey(id))
            return itemDic[id];
        else
            return null;
    }

    public List<ItemData> GetItems(ItemType itemType)
    {
        return items.FindAll(item => item.Type == itemType);
    }
}
