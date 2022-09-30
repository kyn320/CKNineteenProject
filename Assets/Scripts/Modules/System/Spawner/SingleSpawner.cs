using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSpawner : MonoBehaviour
{
    public GameObject spawnPrefab;

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
                Spawn();
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
        return ObjectPoolManager.Instance.Get(spawnPrefab);
    }

    public virtual GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        return ObjectPoolManager.Instance.Get(spawnPrefab, position, rotation);
    }

    public virtual Vector3 GetRandomSpawnPoint()
    {
        return spawnPointList[Random.Range(0, spawnPointList.Count)].position;
    }

    public virtual Vector3 GetSpawnPoint(int index)
    {
        return spawnPointList[index].position;
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
