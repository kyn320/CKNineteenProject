using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealAreaEvent : MonoBehaviour
{
    GameObject player;
    PlayerStatus playerStatus;
    [SerializeField]
    float heallingDelayTime = 2f;
    [SerializeField]
    float heallingDelayTimer;

    [SerializeField]
    GameObject healEffect;
    GameObject healEffectObj;
    [SerializeField]
    float lifeTime;

    private void Start()
    {
        lifeTime = GetComponent<AutoDestroyByLifetime>().lifeTime - 0.5f;
    }

    public void playerSearch(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player = collider.gameObject;
            playerStatus = player.GetComponent<PlayerStatus>();

            if (healEffect)
            {
                healEffectObj = Instantiate(healEffect, player.transform);
            }
        }
    }

    public void Healing(float healPoint)
    {
        if (player)
        {
            heallingDelayTimer += Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
                Destroy(healEffectObj);


                if (heallingDelayTimer > heallingDelayTime)
            {
                Debug.Log($"Healing {player.name}");
                playerStatus.AddHP(healPoint, true);
                player.GetComponent<PlayerStatus>().AddHP(healPoint, true);
                heallingDelayTimer = 0;
            }
        }
    }
    public void playerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (healEffect)
            {
                Destroy(healEffectObj);
            }
        }
    }
}
