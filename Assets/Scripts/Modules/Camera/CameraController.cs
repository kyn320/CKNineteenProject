using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    Transform tr;

    [SerializeField]
    float camSize;

    public Vector3 minMoveArea, maxMoveArea;

    public Transform target;
    public Vector3 minTargetArea, maxTargetArea;

    [SerializeField]
    Vector3 marginToTarget = Vector3.zero;

    public float lerpSpeed = 100f;

    float verticalHeightSeen;
    float verticalWidthSeen;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        camSize = cam.orthographicSize;
        tr = GetComponent<Transform>();

        verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;

        if (target != null)
        {
            ClampTargetArea();
        }

        ClampMoveArea();

    }

    void ClampTargetArea()
    {
        Vector3 pos = tr.position;

        if (pos.x > target.position.x + maxTargetArea.x || pos.x < target.position.x + minTargetArea.x)
            pos.x = Mathf.Clamp(pos.x, target.position.x + minTargetArea.x, target.position.x + maxTargetArea.x);
        if (pos.y > target.position.y + maxTargetArea.y || pos.y < target.position.y + minTargetArea.y)
            pos.y = Mathf.Clamp(pos.y, target.position.y + minTargetArea.y, target.position.y + maxTargetArea.y);

        tr.position = Vector3.Lerp(tr.position, pos + marginToTarget, Mathf.Min(1, lerpSpeed * Time.deltaTime));
    }

    void ClampMoveArea()
    {
        Vector3 pos = tr.position;
        pos.x = Mathf.Clamp(pos.x, minMoveArea.x + verticalWidthSeen * 0.5f, maxMoveArea.x - verticalWidthSeen * 0.5f);
        pos.y = Mathf.Clamp(pos.y, minMoveArea.y + verticalHeightSeen * 0.5f, maxMoveArea.y - verticalHeightSeen * 0.5f);

        tr.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;

        //이동 가능 영역
        Gizmos.color = Color.yellow - new Color(0, 0, 0, 0.5f);
        Gizmos.DrawCube((minMoveArea + maxMoveArea) * 0.5f
            , new Vector3(Mathf.Abs(minMoveArea.x) + Mathf.Abs(maxMoveArea.x)
            , Mathf.Abs(minMoveArea.y) + Mathf.Abs(maxMoveArea.y)));
        Gizmos.color = Color.green - new Color(0, 0, 0, 0.5f);
        Gizmos.DrawWireCube((minMoveArea + maxMoveArea) * 0.5f
            , new Vector3(Mathf.Abs(minMoveArea.x) + Mathf.Abs(maxMoveArea.x)
            , Mathf.Abs(minMoveArea.y) + Mathf.Abs(maxMoveArea.y)));

        //카메라 사이즈
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(verticalWidthSeen, verticalHeightSeen, 0));

        //타겟 이동 허용 범위
        Gizmos.color = Color.red - new Color(0, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position + (maxTargetArea + minTargetArea) * 0.5f, maxTargetArea - minTargetArea);

        //최소, 최대 이동 BoundsPoint
        Gizmos.color = Color.green - new Color(0, 0, 0, 0.5f);
        Gizmos.DrawCube(minMoveArea, Vector3.one);
        Gizmos.DrawCube(maxMoveArea, Vector3.one);
    }
}
