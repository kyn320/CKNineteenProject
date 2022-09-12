using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilboardRenderer : MonoBehaviour
{
    private Transform cameraToLookAt;

    private void Awake()
    {
        cameraToLookAt =  Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0,cameraToLookAt.rotation.eulerAngles.y,0);
    }
}
