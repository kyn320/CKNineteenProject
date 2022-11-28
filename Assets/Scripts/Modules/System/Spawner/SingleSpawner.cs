using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSpawner : MonoBehaviour
{
    public GameObject spawnPrefab;

    public List<Transform> spawnPointList;

    public bool useRandomSpawn = true;
    public bool useAutoSpawnByTime = true;
    public AmountRangeFloat spawnTimeRange;

    protected float currentSpawnTime;

    protected virtual void Start()
    {
        if (useAutoSpawnByTime)
        {
            currentSpawnTime = spawnTimeRange.GetRandomAmount();
        }
    }

    protected virtual void Update()
    {
        if (useAutoSpawnByTime)
        {
            currentSpawnTime -= Time.deltaTime;

            if (currentSpawnTime <= 0)
            {
                currentSpawnTime = spawnTimeRange.GetRandomAmount();
                Spawn();
            }
        }
    }

    public virtual List<GameObject> Spawn()
    {
        var spawnObjectList = new List<GameObject>();
        if (useRandomSpawn)
        {
            var spawnPoint = GetRandomSpawnPoint();
            spawnObjectList.Add(GetObject(spawnPoint.position, spawnPoint.rotation));
        }
        else
        {
            for (var i = 0; i < spawnPointList.Count; ++i)
            {
                var spawnPoint = spawnPointList[i];
                spawnObjectList.Add(GetObject(spawnPoint.position, spawnPoint.rotation));
            }
        }

        return spawnObjectList;
    }

    public virtual GameObject GetObject()
    {
        return Instantiate(spawnPrefab);
        //return ObjectPoolManager.Instance.Get(spawnPrefab);
    }

    public virtual GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        return Instantiate(spawnPrefab, position, rotation);
        //return ObjectPoolManager.Instance.Get(spawnPrefab, position, rotation);
    }

    public virtual Transform GetRandomSpawnPoint()
    {
        return spawnPointList[Random.Range(0, spawnPointList.Count)];
    }

    public virtual Transform GetSpawnPoint(int index)
    {
        return spawnPointList[index];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (var i = 0; i < spawnPointList.Count; ++i)
        {
            Gizmos.DrawSphere(spawnPointList[i].position, 0.5f);
        }
    }

}
