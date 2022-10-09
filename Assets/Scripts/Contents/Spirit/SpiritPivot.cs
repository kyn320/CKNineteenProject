using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPivot : MonoBehaviour
{
    [SerializeField]
    private Transform originAxis;
    public Vector3 originOffset;
    public Vector3 offset;

    private void FixedUpdate()
    {
        transform.position = originAxis.position + originAxis.forward * offset.z + originAxis.right * offset.x + originAxis.up * offset.y;
        transform.forward = originAxis.forward;
    }

    public void SetOriginOffset()
    {
        offset = originOffset;
    }

    public void SetOffset(Vector3 offset)
    {
        this.offset = offset;
    }

}
