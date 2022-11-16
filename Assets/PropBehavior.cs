using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PropBehavior : MonoBehaviour
{
    [SerializeField]
    private float startCount = .0f;

    [SerializeField]
    private float rotationSpeed = .0f;
    [SerializeField]
    private float rotationPower = .0f;
    

    private float rotationCount = .0f;

    private Rigidbody rig;
    private BoxCollider coll;
    private MeshRenderer render;

    bool usedProp = false;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        render = GetComponent<MeshRenderer>();

        if(rig == null || coll == null)
        {
            Debug.Log(" RIG IS NOT FOUND ");
        }

        if (coll == null)
        {
            Debug.Log(" RIG IS NOT FOUND ");
        }
    }


    IEnumerator FadeOut()
    {
        int i = 100;

        while (i > 0)
        {
            Debug.Log(i);

            i -= 1;
            float f = i / 100;
            Color c = render.material.color;
            c.a = f;
            render.material.color = c;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(this.gameObject);
    }

    void Update()
    {
        if(usedProp)
        {
            startCount -= Time.deltaTime;

            if(startCount <= 0)
            {
                rig.constraints = RigidbodyConstraints.None;
                coll.isTrigger = true;
            }
            else
                DownEffect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
        if (collision.transform.tag == "Player")
        {
            usedProp = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            usedProp = false;

            StartCoroutine("FadeOut");
            rig.constraints = RigidbodyConstraints.FreezeAll;
            coll.enabled = false;
        }
    }

    private void DownEffect()
    {
        rotationCount -= Time.deltaTime * rotationSpeed;

        float power = Mathf.Sin(rotationCount) * rotationPower;

        transform.rotation = Quaternion.Euler(new Vector3(power, transform.position.y, transform.position.z));
    }
}
