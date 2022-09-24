using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPositonChanger : MonoBehaviour
{
    public Vector3 testingVecotr;
    public GameObject cameraAnchor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        testingVecotr = cameraAnchor.transform.forward;
        this.gameObject.transform.position = new Vector3(testingVecotr.x, testingVecotr.y + 3, testingVecotr.z);
    }
}
