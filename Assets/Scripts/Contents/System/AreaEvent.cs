using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaEvent : MonoBehaviour
{
    [SerializeField]
    GameObject user;
    [SerializeField]
    PlayerStatus playerStatus;
    [SerializeField]
    string[] userTags;
    [SerializeField]
    float intervalDelayTime = 2f;
    [SerializeField]
    float intervalDelayTimer;

    public UnityEvent<GameObject> IntervalEvents;

    [SerializeField]
    GameObject hitEffect;
    GameObject hitEffectObj;

    float lifeTime;

    private void Start()
    {
        lifeTime = GetComponent<AutoDestroyByLifetime>().lifeTime - 0.5f;
    }

    public void UserSearch(Collider collider)
    {   
        for(int i = 0; i < userTags.Length; i++)
        if (collider.gameObject.tag == userTags[i])
        {
            user = collider.gameObject;
            playerStatus = user.GetComponent<PlayerStatus>();

            if (hitEffect)
            {
                hitEffectObj = Instantiate(hitEffect, user.transform);
            }
        }
    }

    public void IntervalEvent()
    {

        if (user)
        {
            intervalDelayTimer += Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
                Destroy(hitEffectObj);


            if (intervalDelayTimer > intervalDelayTime)
            {
                intervalDelayTimer = 0;
                IntervalEvents?.Invoke(user);
            }
        }
    }

    public void Healing(float healPoint)
    {
        if (user)
        {
            intervalDelayTimer += Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
                Destroy(hitEffectObj);


            if (intervalDelayTimer > intervalDelayTime)
            {
                intervalDelayTimer = 0;
                playerStatus.AddHP(healPoint, true);
                user.GetComponent<PlayerStatus>().AddHP(healPoint, true);
            }
        }
    }
    public void deBuff(BuffData buffdata)
    {
        if (user)
        {
            user.GetComponent<BuffController>().AddBuff(buffdata);
        }
    }
    public void UserExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (hitEffect)
            {
                Destroy(hitEffectObj);
            }
        }
    }
}
