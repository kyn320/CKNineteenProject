using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Data;

[CreateAssetMenu(fileName = "StatusCalculator", menuName = "Status/StatusCalculator", order = 0)]
public class StatusCalculator : ScriptableObject
{
    [SerializeField]
    private string formula;

    private static string formulaSplitChars = @"\+|\-|\*|\/";
    private static string formulaReplacePattern = @"\(|\)";


    [Button("임시 계산")]
    public float Calculate(StatusInfoData infoData)
    {

        if (infoData == null)
            return 0;

        var replaceFormula = Regex.Replace(formula, formulaReplacePattern, "");
        var statusTypeList = Regex.Split(replaceFormula, formulaSplitChars);

        var calculateFormula = formula;
        for (var i = 0; i < statusTypeList.Length; ++i)
        {
            var statusType = (StatusType)Enum.Parse(typeof(StatusType), statusTypeList[i]);
            var amount = infoData.StausDic[statusType].GetAmount();

            calculateFormula = calculateFormula.Replace(statusTypeList[i], amount.ToString());
        }

        DataTable dt = new DataTable();

        var result = dt.Compute(calculateFormula, "").ToString();

        return float.Parse(result);
    }
}
