using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Landmark;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class WorldController : Singleton<WorldController>
{
    [SerializeField]
    private List<MonsterController> monsters;

    [ShowInInspector]
    [ReadOnly]
    private LandmarkController currentActiveLandmark;

    public void AddMonster(MonsterController monster)
    {
        monsters.Add(monster);

        if (currentActiveLandmark != null && currentActiveLandmark.CheckInAlertArea(monster.transform))
        {
            ChangeMonsterTargetToLandmark(monster);
        }
    }

    public void RemoveMonster(MonsterController monster)
    {
        monsters.Remove(monster);
    }

    public void AlertActiveLandmark(LandmarkController landmark)
    {
        currentActiveLandmark = landmark;

        for (var i = 0; i < monsters.Count; ++i)
        {
            var monster = monsters[i];
            if (currentActiveLandmark.CheckInAlertArea(monster.transform))
            {
                ChangeMonsterTargetToLandmark(monster);
            }
        }
    }

    public void ChangeMonsterTargetToLandmark(MonsterController monster)
    {
        if (currentActiveLandmark == null)
            return;

        var target = currentActiveLandmark.CreateTargetPoint(monster.transform);
        monster.SetTarget(target);
        monster.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);
    }

}
