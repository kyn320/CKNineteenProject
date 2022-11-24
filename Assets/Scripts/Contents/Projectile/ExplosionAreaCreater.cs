using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionAreaCreater : MonoBehaviour
{
    [SerializeField]
    GameObject areaEffectObject;
    bool isEffcetInstance = false;

    [SerializeField]
    bool isLowSpawn = true;
    [SerializeField]
    float lowSpawnDistance;

    [SerializeField]
    LayerMask spawnStartLayer;

    float lifeTime = 0;

    private void Start()
    {
        isEffcetInstance = false;
        if (lifeTime == 0)
        lifeTime = GetComponent<AutoDestroyByLifetime>().lifeTime - 0.5f;
    }


    public void CreateArea()
    {
        if (!isEffcetInstance)
        {
            isEffcetInstance = true;
            RaycastHit spawnRay;
            Physics.Raycast(transform.position + new Vector3(0,1f,0), Vector3.down, out spawnRay, 500f, spawnStartLayer);

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
            area.GetComponent<PotionAreaEvent>().SetCalculate(GetComponent<ProjectileController>());

            Destroy(gameObject);
        }
    }
}
