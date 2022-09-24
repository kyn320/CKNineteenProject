using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bullet;
    public float bulletPower;
    public Vector3 bulletStartVecotr;
    public GameObject cameraActhor;



    // Start is called before the first frame update
    void Start()
    {
        if (bullet == null)
        {
            Debug.LogError($"{this.gameObject.name} has not bullet");
        }
        if (cameraActhor == null)
        {
            cameraActhor = transform.Find("Camera Acthor").gameObject;

            if (cameraActhor == null)
                Debug.LogError($"{this.gameObject.name} has not cameraActhor");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 bulletNewVecotr = new Vector3(bulletStartVecotr.x + transform.position.x, bulletStartVecotr.y + transform.position.y, bulletStartVecotr.z + transform.position.z);

            Vector3 cameraFrontVector = cameraActhor.GetComponent<CameraMoveController>().cameraFrontVector;

            GameObject goBullet = Instantiate(bullet, bulletNewVecotr, new Quaternion(0,0,0,0));

            goBullet.GetComponent<Rigidbody>().velocity = cameraFrontVector.normalized * bulletPower;
        }
    }
}
