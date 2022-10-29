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
    private bool useProjectionPosition = true;

    [SerializeField]
    private bool useProjectionRotation = true;

    [SerializeField]
    private Quaternion offsetRotation;

    private void Update()
    {
        if (bezierCurve == null || setupObjectList.Count == 0)
            return;

        if (!useAutoUpdate)
            return;

        var setupCount = setupObjectList.Count;
        var lineCount = bezierCurve.GetLineCount();
        var progressPerObjectCount = (float)lineCount / (setupCount - 1);


        for (var i = 0; i < setupCount; ++i)
        {
            var position = bezierCurve.GetPosition(progressPerObjectCount * i);

            var lookAtPosition = bezierCurve.GetPosition(progressPerObjectCount * i - progressPerObjectCount * 0.5f) -
                bezierCurve.GetPosition(progressPerObjectCount * i + progressPerObjectCount * 0.5f);

            lookAtPosition.Normalize();
            var lookAtDegree = Mathf.Atan2(lookAtPosition.x, lookAtPosition.z) * Mathf.Rad2Deg;

            var projectionInfo = GetProjectionInfo(position);

            if (useProjectionPosition && projectionInfo.collider != null)
            {
                setupObjectList[i].transform.position = projectionInfo.point;
            }
            else
            {
                setupObjectList[i].transform.position = position;
            }

            if (useProjectionRotation && projectionInfo.collider != null)
            {
                var hitNormalRotation = Quaternion.FromToRotation(transform.up, projectionInfo.normal);
                setupObjectList[i].transform.rotation = hitNormalRotation * Quaternion.Euler(0f, lookAtDegree, 0f) * offsetRotation;
            }
            else
            {
                setupObjectList[i].transform.rotation = Quaternion.Euler(0f, lookAtDegree, 0f) * offsetRotation;
            }

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
    public void RemoveAllObjects()
    {
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

    public RaycastHit GetProjectionInfo(Vector3 position)
    {
        RaycastHit rayCastHit;
        Physics.Raycast(position, Vector3.down, out rayCastHit, 10000);

        return rayCastHit;
    }

}
