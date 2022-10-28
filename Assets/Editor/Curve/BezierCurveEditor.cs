using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : OdinEditor
{

    private void OnSceneGUI()
    {
        Tools.current = Tool.None;

        var bezierCurve = target as BezierCurve;

        var pointList = bezierCurve.GetPointList();

        for (var i = 0; i < pointList.Count; ++i)
        {
            var anchor = pointList[i].GetAnchor();
            var leftHandle = pointList[i].GetLeftHandle();
            var rightHandle = pointList[i].GetRightHandle();

            anchor.position = Handles.DoPositionHandle(anchor.position, Quaternion.identity);
            leftHandle.position = Handles.DoPositionHandle(leftHandle.position, Quaternion.identity);
            rightHandle.position = Handles.DoPositionHandle(rightHandle.position, Quaternion.identity);
        }

    }

}
