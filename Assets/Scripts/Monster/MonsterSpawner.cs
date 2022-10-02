using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : SingleSpawner
{
    public void SetSpawnTime(float time)
    {
        maxSpawnTime *= time;
    }
}
