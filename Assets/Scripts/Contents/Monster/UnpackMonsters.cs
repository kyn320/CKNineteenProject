using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpackMonsters : MonoBehaviour
{

    public void Awake()
    {
        var childMonsters = GetComponentsInChildren<MonsterController>();

        foreach (var monster in childMonsters)
        {
            monster.transform.SetParent(null);
        }
    }
}
