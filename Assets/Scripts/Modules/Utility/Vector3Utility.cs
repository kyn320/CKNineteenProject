using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Utility
{
    public static Vector3 AngleToDir(float angle) {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    public static Vector3 RandomRange(Vector3 rangeVector) { 
        var randomVector = Vector3.zero;

        randomVector.x = Random.Range(-rangeVector.x, rangeVector.x);
        randomVector.y = Random.Range(-rangeVector.y, rangeVector.y);
        randomVector.z = Random.Range(-rangeVector.z, rangeVector.z);

        return randomVector;
    }

    public static Vector3 RandomRange(Vector3 minRange, Vector3 maxRange)
    {
        var randomVector = Vector3.zero;

        randomVector.x = Random.Range(minRange.x, maxRange.x);
        randomVector.y = Random.Range(minRange.y, maxRange.y);
        randomVector.z = Random.Range(minRange.z, maxRange.z);

        return randomVector;
    }

}
