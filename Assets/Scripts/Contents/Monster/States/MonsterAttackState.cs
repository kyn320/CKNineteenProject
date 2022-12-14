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
        var target = controller.GetTarget();

        var patternGroup = GetRandomPatternGroup();
        var pattern = patternGroup.AttackPattern;

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

    private MonsterAttackPatternRangeGroup GetRandomPatternGroup()
    {
        var rand = Random.Range(0f, 100f);

        var currentPercent = 0f;

        for (var i = 0; i < patternGroupList.Count; ++i)
        {
            var checkPercent = patternGroupList[i].Percent;

            if (rand < currentPercent + checkPercent)
            {
                return patternGroupList[i];
            }
            else
            {
                currentPercent += checkPercent;
            }
        }

        return null;
    }

}
