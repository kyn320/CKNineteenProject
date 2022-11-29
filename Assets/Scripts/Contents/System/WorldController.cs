using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Landmark;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class WorldController : Singleton<WorldController>
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private List<MonsterController> monsters;
    public int RemainMonsterCount { get { return monsters.Count; } }

    [ShowInInspector]
    [ReadOnly]
    private LandmarkController currentActiveLandmark;
    public LandmarkController ActiveLandmark { get { return currentActiveLandmark; } }

    public bool isPlay = false;
    public UnityEvent endGameEvent;

    private void Start()
    {
        player.deathEvent.AddListener(EndPlay);
    }

    public void EndPlay()
    {
        player.GetInputController().enabled = false;
        isPlay = false;
    }

    public void ClearGame()
    {
        EndPlay();
        UIController.Instance.OpenPopup(new UIGameClearPopupData());
    }

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
            if (currentActiveLandmark == null)
            {
                monster.SetTarget(player.transform);
                monster.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);
            }
            else if (currentActiveLandmark.CheckInAlertArea(monster.transform))
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
