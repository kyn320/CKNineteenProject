using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkStateBase : MonoBehaviour
{
    public Landmark.LandmarkSystem manager;
    private void Awake()
    {
        manager = GetComponent<Landmark.LandmarkSystem>();
    }
    public virtual void Action()
    {

    }

}
