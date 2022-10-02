using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEffect : MonoBehaviour
{
    [SerializeField]
    private int itemCode = 0;

    [SerializeField]
    private GameObject itemBox;


    [SerializeField]
    private float rotationSpeed = .0f;

    private Rigidbody rig;
    private BoxCollider coll;

    private void Awake()
    {
        if(itemBox == null)
        {
            itemBox = transform.GetChild(0).gameObject;
        }

        rig = itemBox.GetComponent<Rigidbody>();
        coll = itemBox.GetComponent<BoxCollider>();

        ShootItemBox();
    }

    private void RotateItemBox()
    {
        itemBox.transform.rotation = Quaternion.Euler(Vector3.left * rotationSpeed * Time.deltaTime);
    }

    private void ShootItemBox()
    {
        rig.GetPointVelocity(transform.TransformPoint(new Vector3(0f, 0.1f, 0f)));
    }

    private void Update()
    {
        RotateItemBox();
    }

}
