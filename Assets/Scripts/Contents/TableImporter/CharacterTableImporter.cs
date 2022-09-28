/*
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CharacterTableImporter", menuName = "Excel/CharacterTableImporter", order = 0)]
public class CharacterTableImporter : ExcelImporter
{
    [FolderPath]
    public string saveFolderPath = "Assets/ScriptableObjects/Character";

    public string characterDataFileName = "CharacterData";
    public string statusInfoDataFileName = "StatusInfoData";

    [FolderPath]
    public string characterObjectFolder= "Assets/Prefabs/Character";

    [FolderPath]
    public string iconFolder = "Assets/Sprites/Icon";

    [FolderPath]
    public string gradeEffectDataFolder = "Assets/ScriptableObjects/Grade";
    public string gradeEffectDataFileName = "GradeEffectData";

    public CharacterDataContainer dataContainer;

    [Button("캐릭터 데이터 생성")]
    public void CreateData()
    {
        dataContainer.CharacterDatas.Clear();

        for (var i = 0; i < loadDataContainer.Count; ++i)
        {
            //요소 접근
            var dataDic = loadDataContainer[i];

            //능력치 정보 생성
            StatusInfoData statusInfoData = CreateInstance<StatusInfoData>();
            statusInfoData.AutoGenerate();
            statusInfoData.StausDic[StatusType.MinMoveSpeed].SetAmount(float.Parse(dataDic["MinMoveSpeed"]));
            statusInfoData.StausDic[StatusType.MaxMoveSpeed].SetAmount(float.Parse(dataDic["MaxMoveSpeed"]));

            statusInfoData.StausDic[StatusType.IncreaseMoveSpeed].SetAmount(float.Parse(dataDic["IncreaseMoveSpeed"]));
            statusInfoData.StausDic[StatusType.DecreaseMoveSpeed].SetAmount(float.Parse(dataDic["DecreaseMoveSpeed"]));

            statusInfoData.StausDic[StatusType.RotationChangeTime].SetAmount(float.Parse(dataDic["RotationChangeTime"]));
            statusInfoData.StausDic[StatusType.RotateSpeed].SetAmount(float.Parse(dataDic["RotateSpeed"]));

            statusInfoData.StausDic[StatusType.KnockBackPower].SetAmount(float.Parse(dataDic["KnockBackPower"]));
            statusInfoData.StausDic[StatusType.KnockBackDefence].SetAmount(float.Parse(dataDic["KnockBackDefence"]));

            var playerObject = (GameObject)AssetDatabase.LoadAssetAtPath($"{characterObjectFolder}/{dataDic["PlayerObject"]}.prefab", typeof(GameObject));
            var aiObject = (GameObject)AssetDatabase.LoadAssetAtPath($"{characterObjectFolder}/{dataDic["AIObject"]}.prefab", typeof(GameObject));
            var gradeEffectData = (GradeEffectData)AssetDatabase.LoadAssetAtPath($"{gradeEffectDataFolder}/{gradeEffectDataFileName}_{dataDic["Grade"]}.asset", typeof(GradeEffectData));
            var icon = (Sprite)AssetDatabase.LoadAssetAtPath($"{iconFolder}/{dataDic["Icon"]}.png", typeof(Sprite));

            //캐릭터 정보 생성
            CharacterData characterData = CreateInstance<CharacterData>();
            characterData.SetData(
                int.Parse(dataDic["ID"]),
                (GradeType)Enum.Parse(typeof(GradeType), dataDic["Grade"]),
                dataDic["Name"],
                "",
                icon,
                playerObject,
                aiObject,
                statusInfoData,
                gradeEffectData
                );

            var identityName = dataDic["IdentityName"];

            //string statusInfoFile = AssetDatabase.GenerateUniqueAssetPath($"{saveFolderPath}/{statusInfoDataFileName}_{identityName}.asset");
            AssetDatabase.CreateAsset(statusInfoData, $"{saveFolderPath}/{statusInfoDataFileName}_{identityName}.asset");

            //string characterFile = AssetDatabase.GenerateUniqueAssetPath($"{saveFolderPath}/{characterDataFileName}_{identityName}.asset");
            AssetDatabase.CreateAsset(characterData, $"{saveFolderPath}/{characterDataFileName}_{identityName}.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(statusInfoData);
            EditorUtility.SetDirty(characterData);

            playerObject.GetComponent<TrackRunner>().SetCharacterData(characterData);
            playerObject.GetComponent<GradeEffectCreator>().SetData(gradeEffectData);

            aiObject.GetComponent<TrackRunner>().SetCharacterData(characterData);
            aiObject.GetComponent<GradeEffectCreator>().SetData(gradeEffectData);


            EditorUtility.SetDirty(playerObject);
            EditorUtility.SetDirty(aiObject);

            dataContainer.CharacterDatas.Add(characterData);
        }

        AssetDatabase.Refresh();
        EditorApplication.RepaintProjectWindow();
        EditorApplication.RepaintHierarchyWindow();
    }

}
#endif
*/