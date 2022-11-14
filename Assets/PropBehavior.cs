using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PropBehavior : MonoBehaviour
{
    [SerializeField]
    private float startCount = .0f;

    private Rigidbody rig;
    private BoxCollider coll;
    private Renderer render;

    bool usedProp = false;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        render = GetComponent<Renderer>();

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
            i -= 1;
            float f = i / 10.0f;
            Color c = render.material.color;
            c.a = f;
            GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(0.02f);
        }

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(usedProp)
        {
            startCount -= Time.deltaTime;

            if(startCount <= 0)
            {
                rig.constraints = RigidbodyConstraints.None;
                StartCoroutine("FadeOut");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            usedProp = true;
        }
    }

    private void DownEffect()
    {

    }

    private void DestroyEffect()
    {

    }

}
