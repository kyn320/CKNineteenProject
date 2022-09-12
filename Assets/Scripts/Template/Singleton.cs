using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤 패턴을 정의한다.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public bool IsDontDestroy = true;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {

                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError("씬 내에 " + typeof(T).ToString() + " 이(가) 존재하지 않습니다.");
                    Debug.Break();
                }

            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (IsDontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}