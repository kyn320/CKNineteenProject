#if UNIRT_EDITOR 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ItemTableImporter", menuName = "Excel/ItemTableImporter", order = 0)]
public class ItemTableImporter : ExcelImporter
{
    [FolderPath]
    public string saveFolderPath = "Assets/ScriptableObjects/Items";

    [FolderPath]
    public string iconFolder = "Assets/Sprites/Icon";

    public ItemDataContainer dataContainer;

    [Button("������ ������ ����")]
    public void CreateData()
    {
        dataContainer.Items.Clear();

        for (var i = 0; i < loadDataContainer.Count; ++i)
        {
            //��� ����
            var dataDic = loadDataContainer[i];

            //�ɷ�ġ ���� ����
            //StatusInfoData statusInfoData = CreateInstance<StatusInfoData>();
            //statusInfoData.AutoGenerate();
            //statusInfoData.StausDic[StatusType.MinMoveSpeed].SetAmount(float.Parse(dataDic["MinMoveSpeed"]));
            //statusInfoData.StausDic[StatusType.MaxMoveSpeed].SetAmount(float.Parse(dataDic["MaxMoveSpeed"]));
            //
            //statusInfoData.StausDic[StatusType.IncreaseMoveSpeed].SetAmount(float.Parse(dataDic["IncreaseMoveSpeed"]));
            //statusInfoData.StausDic[StatusType.DecreaseMoveSpeed].SetAmount(float.Parse(dataDic["DecreaseMoveSpeed"]));
            //
            //statusInfoData.StausDic[StatusType.RotationChangeTime].SetAmount(float.Parse(dataDic["RotationChangeTime"]));
            //statusInfoData.StausDic[StatusType.RotateSpeed].SetAmount(float.Parse(dataDic["RotateSpeed"]));
            //
            //statusInfoData.StausDic[StatusType.KnockBackPower].SetAmount(float.Parse(dataDic["KnockBackPower"]));
            //statusInfoData.StausDic[StatusType.KnockBackDefence].SetAmount(float.Parse(dataDic["KnockBackDefence"]));
            //
            //var playerObject = (GameObject)AssetDatabase.LoadAssetAtPath($"{characterObjectFolder}/{dataDic["PlayerObject"]}.prefab", typeof(GameObject));
            //var aiObject = (GameObject)AssetDatabase.LoadAssetAtPath($"{characterObjectFolder}/{dataDic["AIObject"]}.prefab", typeof(GameObject));
            //var gradeEffectData = (GradeEffectData)AssetDatabase.LoadAssetAtPath($"{gradeEffectDataFolder}/{gradeEffectDataFileName}_{dataDic["Grade"]}.asset", typeof(GradeEffectData));
            var icon = (Sprite)AssetDatabase.LoadAssetAtPath($"{iconFolder}/{dataDic["Icon"]}.png", typeof(Sprite));

           // //ĳ���� ���� ����
           // CharacterData characterData = CreateInstance<CharacterData>();
           // characterData.SetData(
           //     int.Parse(dataDic["ID"]),
           //     (GradeType)Enum.Parse(typeof(GradeType), dataDic["Grade"]),
           //     dataDic["Name"],
           //     "",
           //     icon,
           //     playerObject,
           //     aiObject,
           //     statusInfoData,
           //     gradeEffectData
           //     );

            var identityName = dataDic["IdentityName"];

            //string statusInfoFile = AssetDatabase.GenerateUniqueAssetPath($"{saveFolderPath}/{statusInfoDataFileName}_{identityName}.asset");
            //AssetDatabase.CreateAsset(statusInfoData, $"{saveFolderPath}/{statusInfoDataFileName}_{identityName}.asset");
            //
            ////string characterFile = AssetDatabase.GenerateUniqueAssetPath($"{saveFolderPath}/{characterDataFileName}_{identityName}.asset");
            //AssetDatabase.CreateAsset(characterData, $"{saveFolderPath}/{characterDataFileName}_{identityName}.asset");
            //AssetDatabase.SaveAssets();
            //
            //EditorUtility.SetDirty(statusInfoData);
            //EditorUtility.SetDirty(characterData);
            //
            //playerObject.GetComponent<TrackRunner>().SetCharacterData(characterData);
            //playerObject.GetComponent<GradeEffectCreator>().SetData(gradeEffectData);
            //
            //aiObject.GetComponent<TrackRunner>().SetCharacterData(characterData);
            //aiObject.GetComponent<GradeEffectCreator>().SetData(gradeEffectData);
            //
            //
            //EditorUtility.SetDirty(playerObject);
            //EditorUtility.SetDirty(aiObject);
            //
            //dataContainer.CharacterDatas.Add(characterData);
        }

        AssetDatabase.Refresh();
        EditorApplication.RepaintProjectWindow();
        EditorApplication.RepaintHierarchyWindow();
    }

}
#endif