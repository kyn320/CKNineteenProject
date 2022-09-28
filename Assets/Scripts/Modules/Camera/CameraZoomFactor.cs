using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomFactor : MonoBehaviour
{
    Camera cam;

    public Transform target;
    public float zoomAmount;
    public float minZoomAmount = 5f;
    public float maxZoomAmount = 5f;

    public float zoomSpeed = 1f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Zoom();
    }

    private void Zoom()
    {
        if (target == null)
            return;

        var zoomSize = Mathf.Clamp(Mathf.Abs((target.position.y - transform.position.y)) * zoomAmount
            , minZoomAmount
            , maxZoomAmount);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, Time.deltaTime  * zoomSpeed);

    }



}
