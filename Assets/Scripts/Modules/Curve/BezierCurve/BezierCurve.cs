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
    public bool showOnlyRoot = false;
    [SerializeField]
    private float pointRadius = 0.5f;
    [SerializeField]
    private Color rootAnchorColor = Color.white;
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
        var detailStep = 0.0001f;

        while (searchProgress < GetLineCount())
        {
            searchProgress += detailStep;
            var checkPosition = GetPosition(searchProgress);
            var distance = (checkPosition - startPosition).sqrMagnitude;
            var diff = checkDistance * checkDistance - distance;

            if(diff < 0)
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

    public CurveNearPointData FindProjectionPoint(Vector3 targetPosition, Vector3 downVector)
    {
        //가까운 점
        var nearestPosition = Vector3.zero;
        //가까운 거리
        var nearDistanceSqr = float.MaxValue;
        //근사 값 범위
        var nearProgress = 0f;
        //제일 가까운 Bezier Line
        BezierLine nearLine = null;
        //제일 가까운 Bezier Line Index
        var resultLineIndex = 0;

        //TODO :: 알고리즘 최적화 방법
        for (var i = 0; i < lineList.Count; ++i)
        {
            var searchLine = lineList[i];
            var scaneTime = 1f / (drawDetailCount * 0.5f);

            for (var t = 0f; t <= 1f; t += scaneTime)
            {
                var searchPoint = searchLine.CalculatePoint(t);
                var sqrDistance = (searchPoint - targetPosition).sqrMagnitude;
                if (sqrDistance < nearDistanceSqr)
                {
                    resultLineIndex = i;
                    nearLine = searchLine;
                    nearProgress = t;
                    nearDistanceSqr = sqrDistance;
                    nearestPosition = searchPoint;
                }
            }
        }

        var minAngle = 360f;
        var projectileProgress = 0f;

        //캐싱된 곡선을 기반으로 drawDetailCount 만큼 나누어 곡선 내 모든 점을 다시 추적 후 가까운 점을 캐싱
        for (var i = -250; i <= 250; ++i)
        {
            var t = nearProgress + i * 0.0001f;
            t = Mathf.Clamp01(t);
            var searchPoint = nearLine.CalculatePoint(t);

            var projectionDirection = (searchPoint - targetPosition).normalized;
            var angle = Vector3.Angle(projectionDirection.normalized, downVector);

            if (angle < minAngle)
            {
                minAngle = angle;
                projectileProgress = t;
                nearestPosition = searchPoint;
            }
        }

        return new CurveNearPointData { line = nearLine, progress = resultLineIndex + projectileProgress, position = nearestPosition };
    }

    public CurveNearPointData FindNearestPoint(Vector3 targetPosition)
    {
        searchProgressList.Clear();

        //가까운 점
        var nearestPosition = Vector3.zero;
        //가까운 거리
        var nearDistanceSqr = float.MaxValue;
        //근사 값 범위
        var nearProgress = 0f;
        //제일 가까운 Bezier Line
        BezierLine nearLine = null;
        //제일 가까운 Bezier Line Index
        var resultLineIndex = 0;

        //TODO :: 알고리즘 최적화 방법
        for (var i = 0; i < lineList.Count; ++i)
        {
            var searchLine = lineList[i];
            var scaneTime = 1f / (drawDetailCount * 0.5f);

            for (var t = 0f; t <= 1f; t += scaneTime)
            {
                var searchPoint = searchLine.CalculatePoint(t);
                var sqrDistance = (searchPoint - targetPosition).sqrMagnitude;
                if (sqrDistance < nearDistanceSqr)
                {
                    resultLineIndex = i;
                    nearLine = searchLine;
                    nearProgress = t;
                    nearDistanceSqr = sqrDistance;
                    nearestPosition = searchPoint;
                }
            }
        }

        //캐싱된 곡선을 기반으로 drawDetailCount 만큼 나누어 곡선 내 모든 점을 다시 추적 후 가까운 점을 캐싱
        for (var i = -250; i <= 250; ++i)
        {
            var t = nearProgress + i * 0.001f;
            t = Mathf.Clamp01(t);
            var searchPoint = nearLine.CalculatePoint(t);

            var sqrDistance = (searchPoint - targetPosition).sqrMagnitude;
            if (sqrDistance < nearDistanceSqr)
            {
                nearProgress = t;
                nearDistanceSqr = sqrDistance;
                nearestPosition = searchPoint;
            }
        }
        return new CurveNearPointData { line = nearLine, progress = resultLineIndex + nearProgress, position = nearestPosition };
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

        Gizmos.color = rootAnchorColor;
        Gizmos.DrawWireSphere(transform.position, pointRadius);

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
