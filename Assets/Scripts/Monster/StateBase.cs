using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase : MonoBehaviour
{
    public MonsterStateManager manager;

    public virtual void Action() {
    }

    public void Awake()
    {
        manager = GetComponent<MonsterStateManager>();
    }
}
