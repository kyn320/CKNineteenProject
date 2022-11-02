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

    [SerializeField]
    private List<GameObject> setupObjectList;
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

    private void UpdateGrid()
    {
        if (setupObjectList == null)
            return;

        var setupCount = gridSize.x * gridSize.y;
        setupObjectBound.transform = setupPrefab.transform;
        setupObjectBound.boxCollider = setupPrefab.GetComponent<BoxCollider>();

        var boundSize = setupObjectBound.Size;

        var centerPosition = transform.position
            - new Vector3((spacingPosition.x + boundSize.x) * gridSize.x
            , 0f
            , (spacingPosition.z + boundSize.z) * gridSize.y) * 0.5f;

        var index = 0;

        for (var y = 0; y < gridSize.y; ++y)
        {
            for (var x = 0; x < gridSize.x; ++x)
            {
                var gridPosition = new Vector3(x, 0f, y);

                Vector3 position = centerPosition + new Vector3(boundSize.x * x * 0.5f, 0, boundSize.z * y * 0.5f);

                var projectionInfo = GetProjectionInfo(position);

                if (useProjectionPosition && projectionInfo.collider != null)
                {
                    setupObjectList[index].transform.position = projectionInfo.point + Vector3.Scale(gridPosition, spacingPosition);
                }
                else
                {
                    setupObjectList[index].transform.position = position + Vector3.Scale(gridPosition, spacingPosition);
                }

                if (useProjectionRotation && projectionInfo.collider != null)
                {
                    var hitNormalRotation = Quaternion.FromToRotation(transform.up, projectionInfo.normal);
                    setupObjectList[index].transform.rotation = hitNormalRotation * offsetRotation;
                }
                else
                {
                    setupObjectList[index].transform.rotation = offsetRotation;
                }

                ++index;
            }
        }

    }

    [Button("������Ʈ �� ����")]
    public void UpdateSetupObjects()
    {
        RemoveAllObjects();
        var setupObjectCount = gridSize.x * gridSize.y;
        setupObjectBound = new SetupObjectBound();

        for (var i = 0; i < setupObjectCount; ++i)
        {
            var element = Instantiate(setupPrefab, transform);
            setupObjectList.Add(element);
        }

        UpdateGrid();
    }

    [Button("������Ʈ ��ü ����")]
    public void RemoveAllObjects()
    {
        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            DestroyImmediate(setupObjectList[i]);
        }

        setupObjectList.Clear();
        setupObjectBound = new SetupObjectBound();
    }

    [Button("��ü ������Ʈ Static")]
    public void SetStaticAllObject(bool isStaic)
    {
        for (var i = 0; i < setupObjectList.Count; ++i)
        {
            setupObjectList[i].isStatic = isStaic;
        }
    }

    public RaycastHit GetProjectionInfo(Vector3 position)
    {
        RaycastHit rayCastHit;
        Physics.Raycast(position, Vector3.down, out rayCastHit, 10000);

        return rayCastHit;
    }
}
