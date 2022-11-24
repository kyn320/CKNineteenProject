using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEventController : MonoBehaviour
{
    [SerializeField]
    GameObject[] effects;
    [SerializeField]
    GameObject[] instanceEffects;

    private void Start()
    {
        instanceEffects = new GameObject[effects.Length];
    }
    public void EffectInstance(int effectNum)
    {
        Instantiate(effects[effectNum]);
    }


    /*
    public void effectInstance(int effectNum, Vector3 vector, float destroyTime)
    {
        Instantiate(effects[effectNum], vector, effects[effectNum].transform.rotation);
        StartCoroutine(delayDestroy(effectNum, destroyTime));
    }

    IEnumerator delayDestroy(int effectNum, float time)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.Destroy(instanceEffects[effectNum]);
    }
    */
}
