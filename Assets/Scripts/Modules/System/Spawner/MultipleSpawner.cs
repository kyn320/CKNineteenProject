using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultipleSpawner : MonoBehaviour
{
    public List<GameObject> spawnPrefabList;
    public List<Transform> spawnPointList;

    public bool useRandomSpawn = true;
    public bool useAutoSpawnByTime = true;
    public float minSpawnTime;
    public float maxSpawnTime;

    protected float currentSpawnTime;

    protected virtual void Start()
    {
        if (useAutoSpawnByTime)
        {
            currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    protected virtual void Update()
    {
        if (useAutoSpawnByTime)
        {
            currentSpawnTime -= Time.deltaTime;

            if (currentSpawnTime <= 0)
            {
                currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            }
        }
    }

    public virtual List<GameObject> Spawn()
    {
        var spawnObjectList = new List<GameObject>();
        if (useRandomSpawn)
        {
            spawnObjectList.Add(GetObject(GetRandomSpawnPoint(), Quaternion.identity));
        }
        else
        {
            for (var i = 0; i < spawnPointList.Count; ++i)
            {
                spawnObjectList.Add(GetObject(spawnPointList[i].position, Quaternion.identity));
            }
        }

        return spawnObjectList;
    }

    public virtual GameObject GetObject()
    {
        return ObjectPoolManager.Instance.Get(GetRandomPrefab());
    }

    public virtual GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        return ObjectPoolManager.Instance.Get(GetRandomPrefab(), position, rotation);
    }

    public virtual Vector3 GetRandomSpawnPoint()
    {
        return spawnPointList[Random.Range(0, spawnPointList.Count)].position;
    }

    public virtual Vector3 GetSpawnPoint(int index)
    {
        return spawnPointList[index].position;
    }

    public virtual GameObject GetRandomPrefab() { 
        return spawnPrefabList[Random.Range(0, spawnPrefabList.Count)];
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
