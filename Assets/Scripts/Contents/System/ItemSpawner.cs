using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner
{
    [SerializeField]
    private ItemDataContainer itemDataContainer;

    [SerializeField]
    private List<int> spawnItemList;

    public void SetSpawnItemList(List<int> spawnItemList)
    {
        this.spawnItemList = spawnItemList;
    }

    public void SpawnItem()
    {
        SpawnItem(spawnItemList);
    }

    public void SpawnItem(List<int> spawnItemList)
    {
        for (var i = 0; i < spawnPointList.Count; ++i)
        {
            var spawnPoint = spawnPointList[i];
            var spawnObject = Spawn();
            spawnObject.transform.position = spawnPoint.position;

            var itemGiver = spawnObject.GetComponent<ItemGiver>();
            itemGiver.SetItemData(itemDataContainer.FindItem(spawnItemList[i]));
        }
    }


}
