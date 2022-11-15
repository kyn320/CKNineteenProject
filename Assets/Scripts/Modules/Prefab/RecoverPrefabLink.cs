using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

public class RecoverPrefabLink : MonoBehaviour
{
    public GameObject prefab;

    [Button("Prefab 링크 복구")]
    public void Recover()
    {
        var unPackPrefabs = gameObject.GetComponentsInChildren<Transform>().Where(item => item.name.Contains(prefab.name));

        var reserveDestoryQueue = new Queue<GameObject>();

        foreach (var unpack in unPackPrefabs)
        {
#if UNITY_EDITOR

            var getSource = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(unpack.gameObject);
            var getInstance = UnityEditor.PrefabUtility.GetPrefabInstanceHandle(unpack.gameObject);

            if (getSource == null && getInstance == null)
            {
                var unpackGameObject = unpack.gameObject;

                //프리팹으로 재배치
                var element = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                element.transform.SetParent(unpackGameObject.transform.parent);
                element.transform.position = unpackGameObject.transform.position;
                element.transform.rotation = unpackGameObject.transform.rotation;
                element.transform.localScale = unpackGameObject.transform.localScale;

                //기존 깨진 프리팹은 삭제 대기
                reserveDestoryQueue.Enqueue(unpackGameObject);
            }
#endif
        }

        while (reserveDestoryQueue.Count > 0)
        {
            var destroyObject = reserveDestoryQueue.Dequeue();
            DestroyImmediate(destroyObject);
        }

    }

}
