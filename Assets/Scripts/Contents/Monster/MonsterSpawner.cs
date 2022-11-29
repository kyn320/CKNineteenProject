using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class MonsterSpawner : SingleSpawner
{
    [ReadOnly]
    [SerializeField]
    private int remainSpawnCount;
    public int spawnCount = 3;

    public UnityEvent endSpawnEvent;

    protected override void Start()
    {
        remainSpawnCount = spawnCount;
        base.Start();
    }

    public void SpawnWithOutList()
    {
        Spawn();
    }

    public override List<GameObject> Spawn()
    {
        if (remainSpawnCount <= 0)
            return null;

        var spawnList = base.Spawn();

        --remainSpawnCount;

        if (remainSpawnCount == 0)
            endSpawnEvent.Invoke();

        return spawnList;
    }

}
