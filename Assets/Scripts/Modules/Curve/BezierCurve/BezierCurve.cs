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

    [SerializeField]
    private bool showStepProgress = false;

    [SerializeField]
    [Min(0.1f)]
    private float stepProgressAmount;

    public BezierLine GetLine(float progress)
    {
        var index = (int)progress;

        if (index < 0)
        {
            return lineList[0];
        }
        else if (index >= lineList.Count)
        {
            return lineList[lineList.Count - 1];
        }

        return lineList[index];
    }

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

    public CurveNearPointData GetPositionToDistance(float startProgress, float checkDistance)
    {
        var startPosition = GetPosition(startProgress);

        var searchProgress = startProgress;
        var minDiff = 999f;
        var nearPointData = new CurveNearPointData();

        while (searchProgress < GetLineCount())
        {
            searchProgress += 0.001f;
            var checkPosition = GetPosition(searchProgress);
            var distance = (checkPosition - startPosition).magnitude;
            var diff = checkDistance - distance;

            if (diff < 0)
                break;

            if (diff < minDiff)
            {
                minDiff = diff;
                nearPointData.position = checkPosition;
                nearPointData.progress = searchProgress;
            }
        }

        return nearPointData;
    }

    public CurveNearPointData FindNearestPoint(Vector3 targetPosition)
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
            var searchDistanceSqr = SearchLineBinarySplit(ref searchRange, line, targetPosition, 0.5f, 4, float.MaxValue);

            if (nearDistanceSqr > searchDistanceSqr)
            {
                nearDistanceSqr = searchDistanceSqr;
                progressRange = searchRange;
                searchLine = line;
            }
        }

        var progressDelta = Mathf.Abs(progressRange[0] - progressRange[1]) / 10f;

        var resultProgress = 0f;

        for (var i = 0; i <= 10; ++i)
        {
            var searchPosition = searchLine.CalculatePoint(progressRange[0] + progressDelta * i);

            var distance = (targetPosition - searchPosition).sqrMagnitude;

            if (nearDistanceSqr >= distance)
            {
                nearestPosition = searchPosition;
                nearDistanceSqr = distance;
                resultProgress = progressRange[0] + progressDelta * i;
            }
        }

        return new CurveNearPointData { line = searchLine, progress = resultProgress, position = nearestPosition };
    }

    public float SearchLineBinarySplit(ref float[] progressRange, BezierLine curveLine, Vector3 targetPosition, float progress, int step, float nearDistance)
    {
        step = Mathf.Clamp(step, 4, 64);

        var stepProgress = 1f / step;
        var midProgress = progress;

        var nearestDistanceSqr = nearDistance;
        var nearestProgress = progress;

        var shortDistanceSqr = float.MaxValue;
        var shortProgress = 1f;

        var isUpdateNearest = false;

        for (var i = -1; i < 2; i += 2)
        {
            var searchProgress = Mathf.Clamp01(midProgress + stepProgress * i);

            var searchPosition = curveLine.CalculatePoint(searchProgress);
            var distanceSqr = (targetPosition - searchPosition).sqrMagnitude;

            if (nearestDistanceSqr > distanceSqr)
            {
                isUpdateNearest = true;
                nearestDistanceSqr = distanceSqr;
                nearestProgress = searchProgress;
            }

            if (shortDistanceSqr > distanceSqr)
            {
                shortDistanceSqr = distanceSqr;
                shortProgress = searchProgress;
            }
        }

        if (isUpdateNearest)
        {
            return SearchLineBinarySplit(ref progressRange, curveLine, targetPosition, nearestProgress, step * 2, nearestDistanceSqr);
        }

        progressRange[0] = nearestProgress < shortProgress ? nearestProgress : shortProgress;
        progressRange[1] = nearestProgress > shortProgress ? nearestProgress : shortProgress;

        return nearestDistanceSqr;
    }

    [Button("앵커 추가")]
    public virtual void AddPoint()
    {
        //앵커 생성
        var pointAnchor = new GameObject("CurvePoint_" + (pointList.Count));
        pointAnchor.transform.SetParent(transform);
        var curvePoint = pointAnchor.AddComponent<CurvePoint>();
        curvePoint.SetAnchor(pointAnchor.transform);
        //앵커 위치 로컬로 배치
        pointAnchor.transform.localPosition = Vector3.zero;

        //왼쪽 핸들 생성
        var leftHandle = new GameObject("LeftHandle");
        leftHandle.transform.SetParent(pointAnchor.transform);
        curvePoint.SetLeftHandle(leftHandle.transform);
        //왼쪽 핸들 로컬로 배치
        leftHandle.transform.localPosition = Vector3.left;

        //오른쪽 핸들 생성
        var rightHandle = new GameObject("RightHandle");
        rightHandle.transform.SetParent(pointAnchor.transform);
        curvePoint.SetRightHandle(rightHandle.transform);
        //오른쪽 핸들 로컬로 배치
        rightHandle.transform.localPosition = Vector3.right;

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

    [Button("Linear로 변경")]
    public void ChangeLinear()
    {
        for (var i = 0; i < lineList.Count; ++i)
        {
            var line = lineList[i];

            line.ChangePointsMode(CurvePoint.Mode.Free);

            var leftHandle = line.LeftHandle;
            var rightHandle = line.RightHandle;

            //라인의 방향성 구하기 start => end 로
            var direction = (line.StartPoint.GetAnchorPosition() - line.EndPoint.GetAnchorPosition());

            //라인거리 / 3 값 구하기
            var handlePosition = direction / 3f;

            //각 핸들 방향으로 라인거리 / 3 만큼 위치 하기
            leftHandle.localPosition = -handlePosition;
            rightHandle.localPosition = handlePosition;
        }

    }


    protected virtual void OnDrawGizmos()
    {
        stepProgressAmount = Mathf.Max(stepProgressAmount, 0.1f);

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

        if (showStepProgress && lineList.Count > 0)
        {
            var progress = 0f;
            while (progress < lineList.Count)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(GetPosition(progress), pointRadius);
                progress += stepProgressAmount;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetPosition(lineList.Count), pointRadius);
        }

        for (var i = 0; i < searchProgressList.Count; ++i)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(searchProgressList[i], pointRadius);
        }
    }

}
