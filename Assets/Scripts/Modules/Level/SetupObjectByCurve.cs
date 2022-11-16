using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class SetupObjectByCurve : MonoBehaviour
{
    [System.Serializable]
    public enum SnapAxis
    {
        None,
        Foward,
        Back,
        Right,
        Left,
    }

    [SerializeField]
    private BezierCurve bezierCurve;

    [SerializeField]
    private List<GameObject> setupPrefabList;

    public int setupObjectCount;

    [ReadOnly]
    [SerializeField]
    private List<GameObject> setupObjectList = new List<GameObject>();

    [ReadOnly]
    [SerializeField]
    private List<SetupObjectBound> setupObjectBoundList = new List<SetupObjectBound>();

    [SerializeField]
    private bool useAutoUpdate = true;

    [SerializeField]
    private bool useRotationFromHeight = true;

    [SerializeField]
    private bool useProjectionPosition = true;

    [SerializeField]
    private bool useProjectionRotation = true;

    [SerializeField]
    private bool useSnap = false;

    [SerializeField]
    private bool showObjectBounds = true;

    [SerializeField]
    private SnapAxis snapAxis;

    [SerializeField]
    private float snapOffset = 0f;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Quaternion offsetRotation;

    [SerializeField]
    private Vector3 randomPositionRangeByPivot;
    [SerializeField]
    private Vector3 randomRotaionRangeByAxis;
    [SerializeField]
    private Vector3 randomScaleRange;

    private void Update()
    {
        if (bezierCurve == null || setupObjectList.Count == 0)
            return;

        if (!useAutoUpdate)
            return;

        var setupCount = setupObjectList.Count;
        var lineCount = bezierCurve.GetLineCount();
        var progressPerObjectCount = (float)lineCount / (setupCount - 1);

        SetActiveAllObjects(false);

        for (var i = 0; i < setupCount; ++i)
        {
            Vector3 position = Vector3.zero;
            Vector3 lookAtPosition = Vector3.zero;
            var resultProgress = 0f;

            setupObjectBoundList[i].transform = setupObjectList[i].transform;
            setupObjectBoundList[i].boxCollider = setupObjectList[i].GetComponent<BoxCollider>();

            if (i > 0 && useSnap)
            {
                var prevBound = setupObjectBoundList[i - 1];
                var nextBound = setupObjectBoundList[i];

                var snapPointData = GetSnapPointData(prevBound);
                var farPointData = bezierCurve.GetPositionToDistance(snapPointData.progress, GetBoundDistanceByAxis(nextBound) + snapOffset);

                position = farPointData.position;
                resultProgress = farPointData.progress;
                //Half 값입니다. 0.5 안해도 되요.
                var diffProgress = snapPointData.progress - farPointData.progress;
                lookAtPosition = bezierCurve.GetPosition(farPointData.progress + diffProgress) -
                    bezierCurve.GetPosition(farPointData.progress - diffProgress);
            }
            else
            {
                position = bezierCurve.GetPosition(progressPerObjectCount * i);
                resultProgress = progressPerObjectCount * i;
                lookAtPosition = bezierCurve.GetPosition(progressPerObjectCount * i - progressPerObjectCount * 0.5f) -
                bezierCurve.GetPosition(progressPerObjectCount * i + progressPerObjectCount * 0.5f);
            }

            lookAtPosition.Normalize();

            var lookAtRotation = Quaternion.LookRotation(lookAtPosition, Vector3.up); //Mathf.Atan2(lookAtPosition.x, lookAtPosition.z) * Mathf.Rad2Deg;

            if (!useRotationFromHeight)
            {
                lookAtRotation.x = 0;
                lookAtRotation.z = 0;
            }

            var projectionInfo = GetProjectionInfo(position);

            if (useProjectionPosition && projectionInfo.collider != null)
            {
                setupObjectList[i].transform.position = projectionInfo.point;
                setupObjectList[i].transform.localPosition += Quaternion.LookRotation(setupObjectList[i].transform.forward, Vector3.up) * offsetPosition;
            }
            else
            {
                setupObjectList[i].transform.position = position;
                setupObjectList[i].transform.localPosition += Quaternion.LookRotation(setupObjectList[i].transform.forward, Vector3.up) * offsetPosition;
            }

            if (useProjectionRotation && projectionInfo.collider != null)
            {
                var hitNormalRotation = Quaternion.FromToRotation(transform.up, projectionInfo.normal);
                setupObjectList[i].transform.rotation = hitNormalRotation * lookAtRotation * offsetRotation;
            }
            else
            {
                setupObjectList[i].transform.rotation = lookAtRotation * offsetRotation;
            }


            setupObjectBoundList[i].progress = resultProgress;
            setupObjectBoundList[i].line = bezierCurve.GetLine(resultProgress);
        }

        SetActiveAllObjects(true);
    }

    [Button("랜덤 위치 적용")]
    public void UpdateRandomPositionSetup()
    {
#if UNITY_EDITOR
        useAutoUpdate = false;

        UnityEditor.Undo.SetCurrentGroupName("Setup Random Positions");
        var group = UnityEditor.Undo.GetCurrentGroup();

        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            UnityEditor.Undo.RecordObject(setupObjectList[i].transform, "Setup Random Position");
            setupObjectList[i].transform.position += GetRandomVectorRange(randomPositionRangeByPivot);
        }

        UnityEditor.Undo.CollapseUndoOperations(group);
#endif
    }

    [Button("랜덤 회전 적용")]
    public void UpdateRandomRotationSetup()
    {
#if UNITY_EDITOR
        useAutoUpdate = false;

        UnityEditor.Undo.SetCurrentGroupName("Setup Random Rotations");
        var group = UnityEditor.Undo.GetCurrentGroup();

        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            UnityEditor.Undo.RecordObject(setupObjectList[i].transform, "Setup Random Rotation");
            var randomRotationVector = GetRandomVectorRange(randomRotaionRangeByAxis);
            var randomRotationQuaternion = Quaternion.Euler(randomRotationVector.x, randomRotationVector.y, randomRotationVector.z);
            setupObjectList[i].transform.rotation *= randomRotationQuaternion;
        }
        UnityEditor.Undo.CollapseUndoOperations(group);
#endif
    }

    [Button("랜덤 크기 적용")]
    public void UpdateRandomScaleSetup()
    {
#if UNITY_EDITOR
        useAutoUpdate = false;

        UnityEditor.Undo.SetCurrentGroupName("Setup Random Scales");
        var group = UnityEditor.Undo.GetCurrentGroup();

        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            UnityEditor.Undo.RecordObject(setupObjectList[i].transform, "Setup Random Scale");
            setupObjectList[i].transform.localScale += GetRandomVectorRange(randomScaleRange);
        }
        UnityEditor.Undo.CollapseUndoOperations(group);
#endif
    }

    [Button("오브젝트 재 설정")]
    public void UpdateSetupObjects()
    {
#if UNITY_EDITOR
        RemoveAllObjects();

        for (var i = 0; i < setupObjectCount; ++i)
        {
            var element = UnityEditor.PrefabUtility.InstantiatePrefab(GetRandomObject()) as GameObject;
            element.transform.SetParent(transform);

            setupObjectList.Add(element);
            setupObjectBoundList.Add(new SetupObjectBound());
        }
#endif
    }

    [Button("오브젝트 전체 삭제")]
    public void RemoveAllObjects()
    {
        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            Destroy(setupObjectList[i]);
        }

        setupObjectList.Clear();
        setupObjectBoundList.Clear();
    }

    [Button("전체 오브젝트 Static")]
    public void SetStaticAllObject(bool isStaic)
    {
        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            setupObjectList[i].isStatic = isStaic;
        }
    }

    [Button("설치 된 오브젝트 독립화")]
    public void BreakLinkBySetupObjects(Transform newParents)
    {
#if UNITY_EDITOR
        useAutoUpdate = false;

        UnityEditor.Undo.SetCurrentGroupName("BreakLink SetupObjects");
        var group = UnityEditor.Undo.GetCurrentGroup();

        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            UnityEditor.Undo.SetTransformParent(setupObjectList[i].transform, newParents, "BreakLink SetupObject Element");
            //setupObjectList[i].transform.SetParent(newParents);
        }

        UnityEditor.Undo.RecordObject(this, "Clear SetupObjects");
        setupObjectList.Clear();
        setupObjectBoundList.Clear();

        UnityEditor.Undo.CollapseUndoOperations(group);
#endif
    }

    public GameObject GetRandomObject()
    {
        return setupPrefabList[Random.Range(0, setupPrefabList.Count)];
    }

    public Vector3 GetRandomVectorRange(Vector3 rangeVector)
    {
        return new Vector3(Random.Range(-rangeVector.x * 0.5f, rangeVector.x * 0.5f)
            , Random.Range(-rangeVector.y * 0.5f, rangeVector.y * 0.5f)
            , Random.Range(-rangeVector.z * 0.5f, rangeVector.z * 0.5f));
    }

    public void SetActiveAllObjects(bool isActive)
    {
        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            setupObjectList[i].SetActive(isActive);
        }
    }

    public RaycastHit GetProjectionInfo(Vector3 position)
    {
        RaycastHit rayCastHit;
        Physics.Raycast(position, Vector3.down, out rayCastHit, 10000);

        return rayCastHit;
    }

    public CurveNearPointData GetSnapPointData(SetupObjectBound bound)
    {
        Vector3 snapPosition = bound.Center;
        switch (snapAxis)
        {
            case SnapAxis.None:
                break;
            case SnapAxis.Foward:
                snapPosition = bound.Foward;
                break;
            case SnapAxis.Back:
                snapPosition = bound.Back;
                break;
            case SnapAxis.Right:
                snapPosition = bound.Right;
                break;
            case SnapAxis.Left:
                snapPosition = bound.Left;
                break;
        }

        return bezierCurve.FindNearestPoint(snapPosition);
    }

    public float GetBoundDistanceByAxis(SetupObjectBound bound)
    {
        float distance = 0f;

        switch (snapAxis)
        {
            case SnapAxis.None:
                break;
            case SnapAxis.Foward:
            case SnapAxis.Back:
                distance = bound.Size.z * 0.5f;
                break;
            case SnapAxis.Right:
            case SnapAxis.Left:
                distance = bound.Size.x * 0.5f;
                break;
        }

        return distance;
    }

    private void OnDrawGizmos()
    {
        if (!showObjectBounds)
            return;

        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            var setupObject = setupObjectList[i];
            var bound = setupObjectBoundList[i];

            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.magenta;

            if (setupObjectBoundList[i].boxCollider == null)
                continue;

            var pivots = setupObjectBoundList[i].GetAllPivots();

            for (var k = 0; k < pivots.Length; ++k)
            {
                Gizmos.DrawWireSphere(pivots[k], 0.25f);
            }

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(setupObject.transform.position, setupObject.transform.rotation, setupObject.transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(bound.boxCollider.center, bound.boxCollider.size);


            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.red;

            var nearPointData = GetSnapPointData(bound);
            Gizmos.DrawSphere(nearPointData.position, 0.25f);
        }

    }

}
