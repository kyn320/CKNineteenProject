using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionAreaCreater : MonoBehaviour
{
    [SerializeField]
    GameObject areaEffectObject;

    [SerializeField]
    bool isLowSpawn = true;
    [SerializeField]
    float lowSpawnDistance;

    [SerializeField]
    LayerMask spawnStartLayer;

    float lifeTime = 0;

    private void Start()
    {
        if(lifeTime == 0)
        lifeTime = GetComponent<AutoDestroyByLifetime>().lifeTime - 0.5f;
    }


    public void CreateArea()
    {
        RaycastHit spawnRay;
        Physics.Raycast(transform.position, Vector3.down, out spawnRay, spawnStartLayer);

        GameObject area = this.gameObject;
        if (isLowSpawn)
        {
            area = Instantiate(areaEffectObject, spawnRay.point + new Vector3(0f, 0.001f, 0f), areaEffectObject.transform.rotation);
        }
        else
        {
            area = Instantiate(areaEffectObject, transform.position + new Vector3(0f, 0.001f, 0f), areaEffectObject.transform.rotation);
        }

        area.GetComponent<AutoDestroyByLifetime>().lifeTime = lifeTime;
    }
}
