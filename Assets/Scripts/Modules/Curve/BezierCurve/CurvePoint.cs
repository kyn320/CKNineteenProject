using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CurvePoint : MonoBehaviour
{
    [System.Serializable]
    public enum Mode
    {
        Free,
        Aligned,
    }

    public Mode mode = Mode.Aligned;

    [SerializeField]
    protected Transform anchor;

    [SerializeField]
    protected Transform[] handles = new Transform[2];

    public void SetAnchor(Transform anchor)
    {
        this.anchor = anchor;
    }

    public Transform GetAnchor()
    {
        return anchor;
    }

    public Vector3 GetAnchorPosition()
    {
        return anchor.position;
    }

    public void SetHandles(Transform[] handles)
    {
        this.handles = handles;
    }

    public Transform[] GetHandles()
    {
        return handles;
    }

    public void SetLeftHandle(Transform handle)
    {
        handles[0] = handle;
    }

    public Transform GetLeftHandle()
    {
        return handles[0];
    }

    public void SetRightHandle(Transform handle)
    {
        handles[1] = handle;
    }

    public Transform GetRightHandle()
    {
        return handles[1];
    }

    public void UpdateHandle(int masterHandleIndex)
    {
        var masterHandle = handles[masterHandleIndex];
        var subHandle = handles[masterHandleIndex == 0 ? 1 : 0];

        switch (mode)
        {
            case Mode.Free:
                break;
            case Mode.Aligned:
                var direction = masterHandle.position - anchor.position;
                subHandle.position = anchor.position - direction;
                break;
        }
    }

    private void Update()
    {
        if (handles.Length != 2)
            return;

        for (var i = 0; i < handles.Length; ++i)
        {
            if (handles[i].hasChanged)
            {
                UpdateHandle(i);
                handles[i].hasChanged = false;
                break;
            }
        }
    }

}
