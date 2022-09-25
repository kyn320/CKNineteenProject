using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasExtensions
{

    public static Matrix4x4 GetCanvasMatrix(this Canvas canvas)
    {
        var rect = canvas.transform as RectTransform;

        var matrix = rect.localToWorldMatrix;

        matrix *= Matrix4x4.Translate(-rect.sizeDelta * 0.5f);

        return matrix;
    }

}
