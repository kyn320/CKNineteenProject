using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnPrefab;
    
    public List<Transform> spawnPointList;

    public bool useAutoSpawnByTime = true;
    public float minSpawnTime;
    public float maxSpawnTime;

    protected float currentSpawnTime;

    protected virtual void Start()
    {
        if (useAutoSpawnByTime) {
            currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);    
        }
    }

    protected virtual void Update()
    {
        if (useAutoSpawnByTime) { 
            currentSpawnTime -= Time.deltaTime;

            if(currentSpawnTime <= 0) {
                currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                var cloneObject = Spawn();
                cloneObject.transform.position = GetSpawnPoint();
            }
        }
    }

    public virtual GameObject Spawn() {
        return Instantiate(spawnPrefab);
    }

    public virtual Vector3 GetSpawnPoint() { 
        return spawnPointList[Random.Range(0, spawnPointList.Count)].position;
    }

}
