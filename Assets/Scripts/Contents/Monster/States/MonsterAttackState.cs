using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MonsterAttackState : MonsterStateBase
{
    [SerializeField]
    private List<MonsterAttackPatternRangeGroup> patternGroupList;

    [Button("확률 검증")]
    private void CheckTotalAttackPatternRange()
    {
        if (patternGroupList.Count < 1)
            return;

        var totalPercent = 0f;

        for (var i = 0; i < patternGroupList.Count; ++i)
        {
            totalPercent += patternGroupList[i].Percent;
        }

        if (totalPercent != 100f)
        {
            Debug.LogError($"Monster Attack Pattern 검증 결과 :: 총합이 {totalPercent}% / 100% 입니다.");
        }
        else
        {
            Debug.Log($"Monster Attack Pattern 검증 결과 :: 정상");
        }
    }

    [Button("가중치 정렬")]
    private void SortByAscending()
    {
        patternGroupList.Sort();
    }

    public override void Enter()
    {
        //공격 가능한 패턴이 있는지 체크 하고 없으면 돌려보내요.
        if (!CheckStartAttackPattern()) { 
            controller.ChangeState(MonsterStateType.MONSTERSTATE_CHASE);
            return;
        }

        var target = controller.GetTarget();

        var pattern = GetRandomPattern();

        pattern.StartAttack(target);

        enterEvent?.Invoke();
    }

    public override void Exit()
    {
        exitEvent?.Invoke();
    }

    public override void Update()
    {
        return;
    }

    public bool CheckStartAttackPattern()
    {
        for (var i = 0; i < patternGroupList.Count; ++i)
        {
            var patternGroup = patternGroupList[i];

            if (!patternGroup.AttackPattern.IsCoolDown)
            {
                return true;
            }
        }

        return false;
    }

    private MonsterAttackPattern GetRandomPattern()
    {
        var rand = Random.Range(0f, 100f);

        var currentPercent = 0f;

        for (var i = 0; i < patternGroupList.Count; ++i)
        {
            var patternGroup = patternGroupList[i];

            var checkPercent = patternGroup.Percent;

            if (rand < currentPercent + checkPercent && !patternGroup.AttackPattern.IsCoolDown)
            {
                return patternGroup.AttackPattern;
            }
            else
            {
                currentPercent += checkPercent;
            }
        }

        return null;
    }

}
