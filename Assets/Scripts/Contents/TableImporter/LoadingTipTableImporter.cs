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

    [Button("�� ������ ����")]
    public void CreateData()
    {
        LoadingTipData tipData = CreateInstance<LoadingTipData>();
        var contentList = new List<string>();

        for (var i = 0; i < loadDataContainer.Count; ++i)
        {
            //��� ����
            var dataDic = loadDataContainer[i];
            contentList.Add(dataDic["Content"]);
        }

        //�� ���� ����
        tipData.SetData(contentList);
        AssetDatabase.CreateAsset(tipData, $"{saveFolderPath}/{tipDataFileName}.asset");
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
        EditorApplication.RepaintProjectWindow();
        EditorApplication.RepaintHierarchyWindow();
    }

}
#endif