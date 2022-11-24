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
                Debug.LogWarning($"씬 내에 {typeof(T).ToString()} 이(가) 존재하지 않습니다. \n 싱글톤 초기화 전에 Instance를 접근하는지 확인 해주세요.");
                return null;
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