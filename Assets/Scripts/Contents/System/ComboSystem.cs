using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboSystem : Singleton<ComboSystem>
{
    [SerializeField]
    private int hitCombo;
    [SerializeField]
    private int maxHitCombo;

    public UnityEvent<int, int> updateHitCombo;

    [SerializeField]
    private int killCombo;
    [SerializeField]
    private int maxKillCombo;

    public UnityEvent<int, int> updateKillCombo;

    public void ResetHitCombo()
    {
        hitCombo = 0;
        updateHitCombo?.Invoke(hitCombo, maxHitCombo);
    }

    public void AddHitCombo(int hitCout)
    {
        hitCombo += hitCout;

        if (maxHitCombo < hitCombo)
        {
            maxHitCombo = hitCombo;
        }
        updateHitCombo?.Invoke(hitCombo, maxHitCombo);
    }

    public void ResetKillCombo()
    {
        killCombo = 0;
        updateKillCombo?.Invoke(killCombo, maxKillCombo);
    }

    public void AddKillCombo(int killCount)
    {
        killCombo += killCount;

        if (maxKillCombo < killCombo)
        {
            maxKillCombo = killCombo;
        }
        updateKillCombo?.Invoke(killCombo, maxKillCombo);
    }
}
