using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

public class SaveLoadSystem : Singleton<SaveLoadSystem>
{
    [SerializeField]
    private SaveLoadPathData saveLoadPathData;

    [SerializeField]
    private PlayRecordData saveLoadData;
    public PlayRecordData SaveLoadData { get { return saveLoadData; } }

    public UnityEvent<bool> loadEvent;
    public UnityEvent<PlayRecordData> updateLoadDataEvent;

    public UnityEvent<bool> saveEvent;

    protected override void Awake()
    {
        base.Awake();
        string destination = Application.persistentDataPath + $"/{saveLoadPathData.SaveFileName}.json";

        if (!File.Exists(destination))
        {
            Save();
        }
    }

    private void Start()
    {
        Load();
    }

    [Button("Load PlayRecordData")]
    public bool Load()
    {
        var success = false;

        Debug.Log(Application.persistentDataPath);

        string destination = Application.persistentDataPath + $"/{saveLoadPathData.SaveFileName}.json";

        if (File.Exists(destination))
        {
            try
            {
                var file = File.ReadAllText(destination);
                saveLoadData = JsonConvert.DeserializeObject<PlayRecordData>(file);
                success = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogWarning($"[SaveLoadSystem] :: {saveLoadPathData.SaveFileName}.dat is not found");
        }

        loadEvent.Invoke(success);
        if (success)
        {
            updateLoadDataEvent?.Invoke(saveLoadData);
        }

        return success;
    }

    [Button("Save PlayRecordData")]
    public bool Save()
    {
        var success = false;

        Debug.Log(Application.persistentDataPath);

        try
        {
            if (saveLoadData != null)
            {
                string destination = Application.persistentDataPath + $"/{saveLoadPathData.SaveFileName}.json";
                var jsonData = JsonConvert.SerializeObject(saveLoadData);
                File.WriteAllText(destination, jsonData);
                success = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        saveEvent.Invoke(success);

        return success;
    }

}
