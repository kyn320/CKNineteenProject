using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : OdinEditor
{
    class UndoPointData
    {
        public Vector3 anchor;
        public Vector3 leftHandle;
        public Vector3 rightHandle;
    }

    private List<UndoPointData> changePointList = new List<UndoPointData>();

    private void OnSceneGUI()
    {
        Tools.current = Tool.None;

        var bezierCurve = target as BezierCurve;
        changePointList.Clear();

        EditorGUI.BeginChangeCheck();

        var rootPivot = Handles.PositionHandle(bezierCurve.transform.position, Quaternion.identity);
        var pointList = bezierCurve.GetPointList();

        for (var i = 0; i < pointList.Count; ++i)
        {
            var anchor = pointList[i].GetAnchor();
            var leftHandle = pointList[i].GetLeftHandle();
            var rightHandle = pointList[i].GetRightHandle();

            changePointList.Add(new UndoPointData()
            {
                anchor = Handles.PositionHandle(anchor.position, Quaternion.identity)
            ,
                leftHandle = Handles.PositionHandle(leftHandle.position, Quaternion.identity)
            ,
                rightHandle = Handles.PositionHandle(rightHandle.position, Quaternion.identity)
            });
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.SetCurrentGroupName("Modify BezierCurve");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(bezierCurve.transform, "Modify BezierCurve : rootPivot");
            for (var i = 0; i < pointList.Count; ++i)
            {
                var anchor = pointList[i].GetAnchor();
                var leftHandle = pointList[i].GetLeftHandle();
                var rightHandle = pointList[i].GetRightHandle();


                Undo.RegisterCompleteObjectUndo(anchor.transform, "Modify BezierCurve : anchor");
                Undo.RegisterCompleteObjectUndo(leftHandle.transform, "Modify BezierCurve : leftHandle");
                Undo.RegisterCompleteObjectUndo(rightHandle.transform, "Modify BezierCurve : rightHandle");

                anchor.position = changePointList[i].anchor;
                leftHandle.position = changePointList[i].leftHandle;
                rightHandle.position = changePointList[i].rightHandle;
            }
            bezierCurve.transform.position = rootPivot;
            Undo.CollapseUndoOperations(group);
        }

    }

}
