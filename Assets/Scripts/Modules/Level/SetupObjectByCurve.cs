using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class SetupObjectByCurve : MonoBehaviour
{
    [SerializeField]
    private BezierCurve bezierCurve;

    [SerializeField]
    private List<GameObject> setupPrefabList;

    public int setupObjectCount;

    [SerializeField]
    private List<GameObject> setupObjectList;

    [SerializeField]
    private bool useAutoUpdate = true;

    [SerializeField]
    private Quaternion offsetRotation;

    private void Update()
    {
        if (bezierCurve == null || setupObjectList.Count == 0)
            return;

        if(!useAutoUpdate)
            return;

        var setupCount = setupObjectList.Count;
        var lineCount = bezierCurve.GetLineCount();
        var progressPerObjectCount = (float)lineCount / (setupCount - 1);


        for (var i = 0; i < setupCount; ++i)
        {
            var position = bezierCurve.GetPosition(progressPerObjectCount * i);
            var nextPosition = bezierCurve.GetPosition(progressPerObjectCount * i + 0.001f);

            var lookAtPosition = nextPosition - position;
            lookAtPosition.Normalize();
            var lookAtDegree = Mathf.Atan2(lookAtPosition.x, lookAtPosition.z) * Mathf.Rad2Deg;

            setupObjectList[i].transform.rotation = Quaternion.Euler(0f, lookAtDegree, 0f) * offsetRotation;
            setupObjectList[i].transform.position = position;
        }
    }

    [Button("오브젝트 재 설정")]
    public void UpdateSetupObjects()
    {
        RemoveAllObjects();

        for (var i = 0; i < setupObjectCount; ++i)
        {
            var element = Instantiate(GetRandomObject(), transform);
            setupObjectList.Add(element);
        }
    }

    [Button("오브젝트 전체 삭제")]
    public void RemoveAllObjects() {
        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            DestroyImmediate(setupObjectList[i]);
        }

        setupObjectList.Clear();
    }

    public GameObject GetRandomObject()
    {
        return setupPrefabList[Random.Range(0, setupPrefabList.Count)];
    }

}
