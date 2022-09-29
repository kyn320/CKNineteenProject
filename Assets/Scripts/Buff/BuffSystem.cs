using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct buffState
{
    public string type;
    public float buffEffectTime;
    public float buffCount;
}

[RequireComponent(typeof(BuffTestPlayer))]
public class BuffSystem : MonoBehaviour
{
    BuffTestPlayer player;
    List<buffState> buffList = new List<buffState>();

    List<float> timePos = new List<float>();
    private float timer = .0f;

    private void Awake()
    {
        player = GetComponent<BuffTestPlayer>();

        //CreateBuff("attackSpeed", 120, 5);

    }
    public void CreateBuff(string type, float buffCount, float effectTime)
    {
        buffState buff;
        buff.type = type;
        buff.buffEffectTime = effectTime;
        buff.buffCount = buffCount;

        buffList.Add(buff);

        switch(type)
        {
            case "hp":
                player.hp += buffCount;
                break;
            case "minAttackPower":
                player.minAttackPower += buffCount;
                break;
            case "maxAttackPower":
                player.maxAttackPower += buffCount;
                break;
            case "defence":
                player.defence += buffCount;
                break;
            case "attackSpeed":
                player.attackSpeed += buffCount;
                break;
            case "attackDistance":
                player.attackDistance += buffCount;
                break;
            case "criticalPercent":
                player.criticalPercent += buffCount;
                break;
            case "criticalAttackPower":
                player.criticalAttackPower += buffCount;
                break;
            case "moveSpeed":
                player.moveSpeed += buffCount;
                break;
            case "jumpPower":
                player.jumpPower += buffCount;
                break;
            case "recoverHp":
                player.recoverHp += buffCount;
                break;
            case "perceive":
                player.perceive += buffCount;
                break;
            case "weaponTime":
                player.weapon_time += buffCount;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (buffList.Count <= 0)
            return;
        foreach (var time in buffList)
        {
            if (time.buffEffectTime <= timer)
            {
                switch (time.type)
                {
                    case "hp":
                        player.hp -= time.buffCount;
                        break;
                    case "minAttackPower":
                        player.minAttackPower -= time.buffCount;
                        break;
                    case "maxAttackPower":
                        player.maxAttackPower -= time.buffCount;
                        break;
                    case "defence":
                        player.defence -= time.buffCount;
                        break;
                    case "attackSpeed":
                        player.attackSpeed -= time.buffCount;
                        break;
                    case "attackDistance":
                        player.attackDistance -= time.buffCount;
                        break;
                    case "criticalPercent":
                        player.criticalPercent -= time.buffCount;
                        break;
                    case "criticalAttackPower":
                        player.criticalAttackPower -= time.buffCount;
                        break;
                    case "moveSpeed":
                        player.moveSpeed -= time.buffCount;
                        break;
                    case "jumpPower":
                        player.jumpPower -= time.buffCount;
                        break;
                    case "recoverHp":
                        player.recoverHp -= time.buffCount;
                        break;
                    case "perceive":
                        player.perceive -= time.buffCount;
                        break;
                    case "weaponTime":
                        player.weapon_time -= time.buffCount;
                        break;
                    default:
                        break;
                }
                buffList.Remove(time);
                break;
            }
        }
    }
}

