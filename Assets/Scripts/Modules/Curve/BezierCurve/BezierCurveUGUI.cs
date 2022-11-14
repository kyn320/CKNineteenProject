using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BezierCurveUGUI : BezierCurve
{
    [SerializeField]
    private Canvas rootCanvas;

    public List<Vector3> searchProgressTestList;


    public void SetRootCanvas(Canvas rootCanvas)
    {
        this.rootCanvas = rootCanvas;
    }

    [Button("��Ŀ �߰�")]
    public override void AddPoint()
    {
        //��Ŀ ����
        var pointAnchor = new GameObject("CurvePoint_" + (pointList.Count), typeof(RectTransform));
        pointAnchor.transform.SetParent(transform);
        var curvePoint = pointAnchor.AddComponent<CurvePoint>();
        curvePoint.SetAnchor(pointAnchor.transform);


        //���� �ڵ� ����
        var leftHandle = new GameObject("LeftHandle", typeof(RectTransform));
        leftHandle.transform.SetParent(pointAnchor.transform);
        curvePoint.SetLeftHandle(leftHandle.transform);

        //������ �ڵ� ����
        var rightHandle = new GameObject("RightHandle", typeof(RectTransform));
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

    protected override void OnDrawGizmos()
    {

        if (rootCanvas == null)
            return;

        Gizmos.matrix = rootCanvas.GetCanvasMatrix();

        base.OnDrawGizmos();
    }

}
