using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MonsterAttackState : MonsterStateBase
{
    [SerializeField]
    private List<MonsterAttackPatternRangeGroup> patternGroupList;

    [Button("Ȯ�� ����")]
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
            Debug.LogError($"Monster Attack Pattern ���� ��� :: ������ {totalPercent}% / 100% �Դϴ�.");
        }
        else { 
            Debug.Log($"Monster Attack Pattern ���� ��� :: ����");
        }
    }

    [Button("����ġ ����")]
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
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        return;
    }

    private MonsterAttackPatternRangeGroup GetRandomPatternGroup()
    {
        var rand = Random.Range(0f, 100f);

        for (var i = 0; i < patternGroupList.Count; ++i)
        {
            if (rand < patternGroupList[i].Percent)
            {
                return patternGroupList[i];
            }
        }

        return null;
    }


}