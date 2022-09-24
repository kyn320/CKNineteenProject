using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bullet;
    public float bulletPower;
    public Vector3 bulletStartVecotr;
    public GameObject cameraActhor;
    public GameObject mainCamera;



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

        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");

            if (mainCamera == null)
            Debug.LogError($"{this.gameObject.name} has not mainCamera");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 bulletNewVecotr = bulletStartVecotr + transform.position;
            Vector3 cameraFrontVector = cameraActhor.GetComponent<CameraMoveController>().cameraFrontVector;


            //ÃÑ¾Ë Ãâ·Â À§Ä¡
            GameObject goBullet = Instantiate(bullet, bulletNewVecotr, new Quaternion(0, 0, 0, 0));



            /*
            float x = Mathf.Abs(cameraActhor.transform.localPosition.x + cameraActhor.transform.localPosition.x);
            float y = Mathf.Abs(cameraActhor.transform.localPosition.y + cameraActhor.transform.localPosition.y);
            */

            //ÃÑ¾Ë ¹°¸® º¤ÅÍ
            goBullet.GetComponent<Rigidbody>().velocity = cameraFrontVector.normalized * bulletPower;
        }
    }
}
