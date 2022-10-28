using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BezierCurve : MonoBehaviour
{
    [SerializeField]
    protected List<BezierLine> lineList;
    [SerializeField]
    protected List<CurvePoint> pointList;

    [SerializeField]
    protected int drawDetailCount = 50;

    [SerializeField]
    private float pointRadius = 0.5f;
    [SerializeField]
    private Color anchorColor = Color.white;
    [SerializeField]
    private Color curveLineColor = Color.white;
    [SerializeField]
    private Color handleColor = Color.blue;
    [SerializeField]
    private Color handleLineColor = Color.red;

    [ReadOnly]
    [ShowInInspector]
    protected List<Vector3> searchProgressList = new List<Vector3>();

    public Vector3 GetPosition(float progress)
    {
        var index = (int)progress;
        return GetPosition(index, progress - index);
    }

    public Vector3 GetPosition(int index, float progress)
    {
        if (index < 0)
        {
            return lineList[0].CalculatePoint(0f);
        }
        else if (index >= lineList.Count)
        {
            return lineList[lineList.Count - 1].CalculatePoint(1f);
        }

        return lineList[index].CalculatePoint(progress);
    }

    public int GetLineCount()
    {
        return lineList.Count;
    }

    public List<CurvePoint> GetPointList()
    {
        return pointList;
    }

    [Button("앵커 추가")]
    public virtual void AddPoint()
    {
        //앵커 생성
        var pointAnchor = new GameObject("CurvePoint_" + (pointList.Count));
        pointAnchor.transform.SetParent(transform);
        var curvePoint = pointAnchor.AddComponent<CurvePoint>();
        curvePoint.SetAnchor(pointAnchor.transform);


        //왼쪽 핸들 생성
        var leftHandle = new GameObject("LeftHandle");
        leftHandle.transform.SetParent(pointAnchor.transform);
        curvePoint.SetLeftHandle(leftHandle.transform);

        //오른쪽 핸들 생성
        var rightHandle = new GameObject("RightHandle");
        rightHandle.transform.SetParent(pointAnchor.transform);
        curvePoint.SetRightHandle(rightHandle.transform);

        pointList.Add(curvePoint);

        if (pointList.Count > 1)
        {
            var startIndex = pointList.Count - 2;
            var bezierLine = new BezierLine();

            bezierLine.points[0] = pointList[startIndex];
            bezierLine.points[1] = pointList[startIndex + 1];

            lineList.Add(bezierLine);
        }

    }

    [Button("루프 묶기")]
    public void LinkLoop()
    {

        if (pointList.Count < 2)
            return;

        var bezierLine = new BezierLine();

        bezierLine.points[0] = pointList[pointList.Count - 1];
        bezierLine.points[1] = pointList[0];

        lineList.Add(bezierLine);
    }

    protected virtual void OnDrawGizmos()
    {
        for (var i = 0; i < pointList.Count; ++i)
        {
            var point = pointList[i];

            Gizmos.color = anchorColor;
            Gizmos.DrawWireSphere(point.GetAnchorPosition(), pointRadius);

            Gizmos.color = handleColor;
            Gizmos.DrawWireSphere(point.GetLeftHandle().position, pointRadius);
            Gizmos.DrawWireSphere(point.GetRightHandle().position, pointRadius);

            Gizmos.color = handleLineColor;
            Gizmos.DrawLine(point.GetAnchorPosition(), point.GetLeftHandle().position);
            Gizmos.DrawLine(point.GetAnchorPosition(), point.GetRightHandle().position);
        }

        for (var i = 0; i < lineList.Count; ++i)
        {
            var curveLine = lineList[i];
            var prevPoint = curveLine.CalculatePoint(0f);

            Gizmos.color = curveLineColor;
            for (var k = 1; k <= drawDetailCount; ++k)
            {
                var nextPoint = curveLine.CalculatePoint(k / (float)drawDetailCount);
                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }
        }

        for (var i = 0; i < searchProgressList.Count; ++i)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(searchProgressList[i], 1f);
        }
    }

    public Vector3 FindNearestPoint(Vector3 targetPosition)
    {
        searchProgressList.Clear();

        var nearestPosition = Vector3.zero;
        var nearDistanceSqr = float.MaxValue;
        var progressRange = new float[2] { 0f, 0f };

        BezierLine searchLine = null;

        for (var i = 0; i < lineList.Count; ++i)
        {
            var line = lineList[i];
            var searchRange = new float[2] { 0f, 0f };
            var searchDistance = SearchLineBinarySplit(ref searchRange, line, targetPosition, 0.5f, 4, float.MaxValue);

            if (nearDistanceSqr > searchDistance)
            {
                nearDistanceSqr = searchDistance;
                progressRange = searchRange;
                searchLine = line;
            }
        }

        var progressDelta = Mathf.Abs(progressRange[0] - progressRange[1]) / 10f;

        for (var i = 0; i <= 10; ++i)
        {
            var searchPosition = searchLine.CalculatePoint(progressRange[0] + progressDelta * i);

            var distance = (targetPosition - searchPosition).sqrMagnitude;


            if (nearDistanceSqr >= distance)
            {
                nearestPosition = searchPosition;
                nearDistanceSqr = distance;
            }
        }

        Debug.Log("====END LOOP====");
        return nearestPosition;
    }

    public float SearchLineBinarySplit(ref float[] progressRange, BezierLine curveLine, Vector3 targetPosition, float progress, int step, float nearDistance)
    {
        step = Mathf.Clamp(step, 4, 64);

        var stepProgress = 1f / step;
        var midProgress = progress;

        var nearestDistance = nearDistance;
        var nearestProgress = progress;

        var shortDistance = float.MaxValue;
        var shortProgress = 1f;

        var isUpdateNearest = false;

        for (var i = -1; i < 2; i += 2)
        {
            var searchProgress = Mathf.Clamp01(midProgress + stepProgress * i);

            var searchPosition = curveLine.CalculatePoint(searchProgress);
            var distance = (targetPosition - searchPosition).sqrMagnitude;

            if (nearestDistance > distance)
            {
                isUpdateNearest = true;
                nearestDistance = distance;
                nearestProgress = searchProgress;
            }

            if (shortDistance > distance)
            {
                shortDistance = distance;
                shortProgress = searchProgress;
            }
        }

        if (isUpdateNearest)
        {
            return SearchLineBinarySplit(ref progressRange, curveLine, targetPosition, nearestProgress, step * 2, nearestDistance);
        }

        progressRange[0] = nearestProgress < shortProgress ? nearestProgress : shortProgress;
        progressRange[1] = nearestProgress > shortProgress ? nearestProgress : shortProgress;

        return nearestDistance;
    }

}
