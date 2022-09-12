using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BezierLine : CurveLine
{
    public BezierLine prevLine;

    public Transform LeftHandle { 
        get { return StartPoint.GetLeftHandle(); }
    }

    public Transform RightHandle { 
        get { return EndPoint.GetRightHandle(); }
    }

    public override Vector3 CalculatePoint(float t)
    {
        var u = 1 - t;
        var tt = t * t;
        var uu = u * u;
        var uuu = uu * u;
        var ttt = tt * t;

        Vector3 p = uuu * StartPoint.GetAnchorPosition();

        if (prevLine != null)
        {
            p += 3 * uu * t * StartPoint.GetRightHandle().position;
        }
        else
        {
            p += 3 * uu * t * StartPoint.GetLeftHandle().position;
        }

        p += 3 * u * tt * EndPoint.GetRightHandle().position;
        p += ttt * EndPoint.GetAnchorPosition();

        return p;
    }
}
