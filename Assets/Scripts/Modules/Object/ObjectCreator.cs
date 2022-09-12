using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public GameObject createPrefab;

    public void Create()
    {
        var cloneObject = ObjectPoolManager.Instance.Get(createPrefab.name);
        cloneObject.transform.position = transform.position;
    }

    public void Create(Vector3 position)
    {
        var cloneObject = ObjectPoolManager.Instance.Get(createPrefab.name);
        cloneObject.transform.position = position;
    }

}
