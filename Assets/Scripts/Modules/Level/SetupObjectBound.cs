using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SetupObjectBound
{
    public string name;
    public CurveLine line;
    public float progress;
    public Transform transform;
    public BoxCollider boxCollider;
    public Vector3 Center
    {
        get
        {
            return transform.position
+ transform.up * boxCollider.center.y
+ transform.forward * boxCollider.center.z
+ transform.right * boxCollider.center.x;
        }
    }

    public Vector3 Size { get { return Vector3.Scale(boxCollider.size , transform.lossyScale); } }
    public Vector3 Foward { get { return Center + (transform.forward * Size.z * 0.5f); } }
    public Vector3 Back { get { return Center + (-transform.forward * Size.z * 0.5f); } }
    public Vector3 Left { get { return Center + (-transform.right * Size.x * 0.5f); } }
    public Vector3 Right { get { return Center + (transform.right * Size.x * 0.5f); } }
    public Vector3 Up { get { return Center + (transform.up * Size.y * 0.5f); } }
    public Vector3 Down { get { return Center + (-transform.up * Size.y * 0.5f); } }

    public Vector3[] GetAllPivots()
    {
        return new Vector3[] {
            Foward,
            Back,
            Left,
            Right,
            Up,
            Down
        };
    }
}