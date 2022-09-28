#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "LoadingTipTableImporter", menuName = "Excel/LoadingTipTableImporter", order = 0)]
public class LoadingTipTableImporter : ExcelImporter
{
    [FolderPath]
    public string saveFolderPath = "Assets/ScriptableObjects/Tip";

    public string tipDataFileName = "TipData";

    [Button("팁 데이터 생성")]
    public void CreateData()
    {
        LoadingTipData tipData = CreateInstance<LoadingTipData>();
        var contentList = new List<string>();

        for (var i = 0; i < loadDataContainer.Count; ++i)
        {
            //요소 접근
            var dataDic = loadDataContainer[i];
            contentList.Add(dataDic["Content"]);
        }

        //팁 정보 생성
        tipData.SetData(contentList);
        AssetDatabase.CreateAsset(tipData, $"{saveFolderPath}/{tipDataFileName}.asset");
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
        EditorApplication.RepaintProjectWindow();
        EditorApplication.RepaintHierarchyWindow();
    }

}
#endif