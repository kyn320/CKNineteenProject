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

    public void RandomSpawnItem() {
        for (var i = 0; i < spawnPointList.Count; ++i)
        {
            var spawnPoint = spawnPointList[i];
            var spawnObject = Spawn();
            spawnObject.transform.position = spawnPoint.position;

            var itemGiver = spawnObject.GetComponent<ItemGiver>();

            var randomItem = itemDataContainer.FindItem(Random.Range(0, itemDataContainer.Items.Count));
            itemGiver.SetItemData(randomItem);
        }
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

    private void OnDrawGizmos()
    {
        if (spawnPointList.Count < 1)
            return;

        Gizmos.color = Color.cyan;
        for (var i = 0; i < spawnPointList.Count; ++i)
        {
            Gizmos.DrawSphere(spawnPointList[i].position, 0.5f);
        }
    }
}
