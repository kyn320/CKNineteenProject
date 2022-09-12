using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour
{
    private List<SoundPoolPlayer> poolList = new List<SoundPoolPlayer>();

    public void Check(AudioClip audio, SoundPoolPlayer player)
    {
        if (poolList.Count > 5)
            poolList.RemoveAt(poolList.Count - 1);

        poolList.Add(player);

        player.Play(audio);
    }

    public void Stop(SoundPoolPlayer player)
    {
        for(int i=0;i<poolList.Count;i++)
        {
            if(poolList[i].name == player.name)
            {
                poolList[i].Stop();
            }
        }
    }
}
