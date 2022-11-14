using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CurveLine
{
    public CurvePoint[] points = new CurvePoint[2];

    public CurvePoint StartPoint
    {
        get { return points[0]; }
    }

    public CurvePoint EndPoint
    {
        get { return points[1]; }
    }

    public void ChangePointsMode(CurvePoint.Mode mode)
    {
        for (var i = 0; i < points.Length; ++i)
        {
            points[i].SetMode(mode);
        }
    }

    public bool ContainsPoint(CurvePoint curvePoint)
    {
        for (var i = 0; i < points.Length; ++i)
        {
            if (points[i] == curvePoint)
            {
                return true;
            }
        }

        return false;
    }

    public abstract Vector3 CalculatePoint(float t);
}
