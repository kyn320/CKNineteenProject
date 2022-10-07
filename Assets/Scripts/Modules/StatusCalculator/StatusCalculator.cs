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

    private static string formulaSplitChars = @"\+|\-|\*|\/|\~";
    private static string formulaReplacePattern = @"\(|\)";
    private static string formulaRandomPattern = @"(\d+)([~])(\d+)";

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
           
            Debug.Log(statusType);
            if(!infoData.StausDic.ContainsKey(statusType))
                continue;

            var amount = infoData.StausDic[statusType].CalculateTotalAmount();

            calculateFormula = calculateFormula.Replace(statusTypeList[i], amount.ToString());
        }

        DataTable dt = new DataTable();

        foreach (Match match in Regex.Matches(calculateFormula, formulaRandomPattern))
        {
            var value1 = Int32.Parse(match.Groups[1].Value);
            var value2 = Int32.Parse(match.Groups[3].Value);

            calculateFormula = Regex.Replace(calculateFormula, formulaRandomPattern, $"{UnityEngine.Random.Range(value1, value2)}");
        }

        Debug.Log(calculateFormula);

        var result = dt.Compute(calculateFormula, "").ToString();

        return float.Parse(result);
    }

    public float Calculate(StatusInfo info)
    {
        if (info == null)
            return 0;

        var replaceFormula = Regex.Replace(formula, formulaReplacePattern, "");
        var statusTypeList = Regex.Split(replaceFormula, formulaSplitChars);

        var calculateFormula = formula;
        for (var i = 0; i < statusTypeList.Length; ++i)
        {
            var statusType = (StatusType)Enum.Parse(typeof(StatusType), statusTypeList[i]);

            Debug.Log(statusType);
            if (!info.ContainsElement(statusType))
                continue;

            var amount = info.GetElement(statusType).CalculateTotalAmount();

            calculateFormula = calculateFormula.Replace(statusTypeList[i], amount.ToString());
        }

        DataTable dt = new DataTable();

        foreach (Match match in Regex.Matches(calculateFormula, formulaRandomPattern))
        {
            var value1 = Int32.Parse(match.Groups[1].Value);
            var value2 = Int32.Parse(match.Groups[3].Value);

            calculateFormula = Regex.Replace(calculateFormula, formulaRandomPattern, $"{UnityEngine.Random.Range(value1, value2)}");
        }

        Debug.Log(calculateFormula);

        var result = dt.Compute(calculateFormula, "").ToString();

        return float.Parse(result);
    }

}
