using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtension
{
    public static T GetOrAddComponent<T>(this Component co) where T : Component
    {
        T component = co.GetComponent<T>();

        if (component == null)
            component = co.gameObject.AddComponent<T>();

        return component;
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

}
