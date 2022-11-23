using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyByLifetime : MonoBehaviour
{

    public float lifeTime;
    private float currentTime;

    private void OnEnable()
    {
        currentTime = lifeTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if(currentTime  <= 0) { 
            Destroy(gameObject);
        }
    }

}
