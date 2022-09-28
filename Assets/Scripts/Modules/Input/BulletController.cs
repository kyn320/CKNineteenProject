using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraAnchor;
    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletStratObject;
    public float bulletPower;
    public Vector3 bulletBurstVector;



    // Start is called before the first frame update
    void Start()
    {
        if (bullet == null)
        {
            Debug.LogError($"{this.gameObject.name} has not bullet");
        }

        if (cameraAnchor == null)
        {
            cameraAnchor = GameObject.Find("Camera Anchor");

            if (cameraAnchor == null)
                Debug.LogError($"{this.gameObject.name} has not cameraAnchor");
        }

        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");

            if (mainCamera == null)
            Debug.LogError($"{this.gameObject.name} has not mainCamera");
        }
    }
    


    void Update()
    {
            /*
            float x = Mathf.Abs(cameraActhor.transform.localPosition.x + cameraActhor.transform.localPosition.x);
            float y = Mathf.Abs(cameraActhor.transform.localPosition.y + cameraActhor.transform.localPosition.y);
            */

            RaycastHit bulletBurstRay;
            Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out bulletBurstRay);
            Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 50000, Color.blue);


            if (bulletBurstRay.point == Vector3.zero)
            {
                bulletBurstVector = mainCamera.transform.position + mainCamera.transform.forward * 50000;
            }
            else
            {
                //Debug.Log($"bulletBurstRay.point : {bulletBurstRay.point}");
                bulletBurstVector =  (bulletStratObject.transform.position - bulletBurstRay.point) * -1;
            }

        if (Input.GetMouseButtonDown(0))
        {
            //ÃÑ¾Ë Ãâ·Â À§Ä¡
            GameObject goBullet = Instantiate(bullet, bulletStratObject.transform.position, new Quaternion(0, 0, 0, 0));

            //ÃÑ¾Ë ¹°¸® º¤ÅÍ
            goBullet.GetComponent<Rigidbody>().velocity = bulletBurstVector.normalized * bulletPower;
        }
    }
}
