using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class SetupObjectByGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject setupPrefab;

    [SerializeField]
    private Vector2Int gridSize;

    [ReadOnly]
    [SerializeField]
    private List<GameObject> setupObjectList = new List<GameObject>();

    [ReadOnly]
    [SerializeField]
    private SetupObjectBound setupObjectBound;

    [SerializeField]
    private bool useAutoUpdate = true;

    [SerializeField]
    private bool useProjectionPosition = true;

    [SerializeField]
    private bool useProjectionRotation = true;

    [SerializeField]
    private bool showObjectBounds = true;

    [SerializeField]
    private Vector3 spacingPosition;

    [SerializeField]
    private Quaternion offsetRotation;

    [SerializeField]
    private bool useForceScale = false;
    [SerializeField]
    private Vector3 forceScale = Vector3.one;

    private void Update()
    {
        if (useAutoUpdate)
        {
            UpdateGrid();
        }
    }

    private void UpdateGrid()
    {
        if (setupObjectList == null)
            return;

        var setupCount = gridSize.x * gridSize.y;
        setupObjectBound.transform = setupPrefab.transform;
        setupObjectBound.boxCollider = setupPrefab.GetComponent<BoxCollider>();


        var boundSize = Vector3.one;

        if (useForceScale)
            boundSize = Vector3.Scale(setupObjectBound.boxCollider.size, forceScale);
        else
            boundSize = setupObjectBound.Size;

        var centerPosition = transform.position
            - transform.right * (-boundSize.x * 0.5f
            + boundSize.x * gridSize.x * 0.5f
            - spacingPosition.x * 1.5f + spacingPosition.x * (gridSize.x - 1 > 0 ? gridSize.x - 1 : 0))

            - transform.forward * (-boundSize.z * 0.5f
            + boundSize.z * gridSize.y * 0.5f
            - spacingPosition.z * 1.5f + spacingPosition.z * (gridSize.y - 1 > 0 ? gridSize.y - 1 : 0));

        var index = 0;

        for (var y = 0; y < gridSize.y; ++y)
        {
            for (var x = 0; x < gridSize.x; ++x)
            {
                Vector3 position = centerPosition + (transform.right * (boundSize.x + spacingPosition.x) * x) + (transform.forward * (boundSize.z + spacingPosition.z) * y);

                var projectionInfo = GetProjectionInfo(position);

                if (useProjectionPosition && projectionInfo.collider != null)
                {
                    setupObjectList[index].transform.position = projectionInfo.point;
                }
                else
                {
                    setupObjectList[index].transform.position = position;
                }

                if (useProjectionRotation && projectionInfo.collider != null)
                {
                    var hitNormalRotation = Quaternion.FromToRotation(transform.up, projectionInfo.normal);
                    setupObjectList[index].transform.rotation = hitNormalRotation * offsetRotation;
                }
                else
                {
                    setupObjectList[index].transform.rotation = transform.rotation * offsetRotation;
                }

                if (useForceScale)
                    setupObjectList[index].transform.localScale = forceScale;
                else
                    setupObjectList[index].transform.localScale = setupPrefab.transform.localScale;

                ++index;
            }
        }

    }

    [Button("오브젝트 재 설정")]
    public void UpdateSetupObjects()
    {
#if UNITY_EDITOR
        RemoveAllObjects();
        var setupObjectCount = gridSize.x * gridSize.y;
        setupObjectBound = new SetupObjectBound();

        for (var i = 0; i < setupObjectCount; ++i)
        {
            var element = UnityEditor.PrefabUtility.InstantiatePrefab(setupPrefab) as GameObject;
            element.transform.SetParent(transform);

            setupObjectList.Add(element);
        }

        UpdateGrid();
#endif
    }

    [Button("전체 높이 위치 수정")]
    public void ChangeYPosition(float yPos)
    {
#if UNITY_EDITOR
        UnityEditor.Undo.SetCurrentGroupName("Setup Height");
        var group = UnityEditor.Undo.GetCurrentGroup();

        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            UnityEditor.Undo.RecordObject(setupObjectList[i].transform, "Setup Height");
            var position = setupObjectList[i].transform.localPosition;
            position.y = yPos;
            setupObjectList[i].transform.localPosition = position;
        }
        UnityEditor.Undo.CollapseUndoOperations(group);
#endif
    }

    [Button("오브젝트 전체 삭제")]
    public void RemoveAllObjects()
    {
        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            DestroyImmediate(setupObjectList[i]);
        }

        setupObjectList.Clear();
        setupObjectBound = new SetupObjectBound();
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
        }

        UnityEditor.Undo.RecordObject(this, "Clear SetupObjects");
        setupObjectList.Clear();
        setupObjectBound = null;

        UnityEditor.Undo.CollapseUndoOperations(group);
#endif
    }

    public RaycastHit GetProjectionInfo(Vector3 position)
    {
        RaycastHit rayCastHit;
        Physics.Raycast(position, Vector3.down, out rayCastHit, 10000);

        return rayCastHit;
    }
}
