using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField]
    private bool showLog = false;

    public int defaultAmount = 10;
    public GameObject[] poolList;
    public int[] poolAmount;

    public SerializableDictionary<string, ObjectPool> objectPoolList = new SerializableDictionary<string, ObjectPool>();

    protected override void Awake()
    {
        base.Awake();
        InitObjectPool();
    }

    void InitObjectPool()
    {
        for (int i = 0; i < poolList.Length; ++i)
        {
            var objectPool = CreatePool(poolList[i]);

            int amount = defaultAmount;

            if (poolAmount.Length > i && poolAmount[i] != 0)
            {
                amount = poolAmount[i];
            }

            for (int j = 0; j < amount; ++j)
            {
                GameObject inst = Instantiate(objectPool.source, objectPool.folder.transform);
                inst.SetActive(false);
                objectPool.unusedList.Add(inst);
            }
            objectPool.maxAmount = amount;
        }
    }

    protected ObjectPool CreatePool(GameObject originSource) {
        ObjectPool objectPool = new ObjectPool();
        objectPool.source = originSource;
        objectPoolList[originSource.name] = objectPool;

        GameObject folder = new GameObject();
        folder.name = originSource.name;
        folder.transform.parent = this.transform;
        objectPool.folder = folder;

        return objectPool;
    }


    public GameObject Get(GameObject originSource) {
        if (!objectPoolList.ContainsKey(originSource.name))
        {
            if (showLog)
                Debug.LogError("[ObjectPoolManager] Can't Find ObjectPool : " + originSource.name);

            CreatePool(originSource);
        }

        return Get(originSource.name);
    }

    public GameObject Get(GameObject originSource, Vector3 position, Quaternion rotation)
    {
        if (!objectPoolList.ContainsKey(originSource.name))
        {
            if (showLog)
                Debug.LogError("[ObjectPoolManager] Can't Find ObjectPool : " + originSource.name);

            CreatePool(originSource);
        }

        return Get(originSource.name, position, rotation);
    }

    public GameObject Get(string name)
    {
        if (!objectPoolList.ContainsKey(name))
        {
            if (showLog)
                Debug.LogError("[ObjectPoolManager] Can't Find ObjectPool : " + name);
        }

        ObjectPool pool = objectPoolList[name];
        if (pool.unusedList.Count > 0)
        {
            GameObject obj = pool.unusedList[0];
            pool.unusedList.RemoveAt(0);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(pool.source, pool.folder.transform);

            ++pool.maxAmount;
            if (showLog)
                Debug.Log(name + " / Pool Size" + pool.maxAmount);
            return obj;
        }
    }

    public GameObject Get(string name, Vector3 position, Quaternion rotation)
    {
        if (!objectPoolList.ContainsKey(name))
        {
            if (showLog)
                Debug.LogError("[ObjectPoolManager] Can't Find ObjectPool : " + name);
            return null;
        }

        ObjectPool pool = objectPoolList[name];
        if (pool.unusedList.Count > 0)
        {
            GameObject obj = pool.unusedList[0];
            pool.unusedList.RemoveAt(0);

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(pool.source, position, rotation, pool.folder.transform);
            ++pool.maxAmount;
            if (showLog)
                Debug.Log(name + " / Pool Size" + pool.maxAmount);

            return obj;
        }
    }

    public void Free(GameObject obj)
    {
        string keyName = obj.transform.name.Replace("(Clone)", "");
        if (!objectPoolList.ContainsKey(keyName))
        {
            if (showLog)
                Debug.LogError("[ObjectPoolManager] Can't Find ObjectPool : " + keyName);
            return;
        }

        ObjectPool pool = objectPoolList[keyName];
        if (pool.unusedList.Contains(obj))
        {
            return;
        }

        obj.transform.SetParent(pool.folder.transform);
        obj.SetActive(false);
        pool.unusedList.Add(obj);
    }

}