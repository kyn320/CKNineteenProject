using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    protected bool isUpdate = true;

    public virtual void Play()
    {
        isUpdate = true;
    }

    public virtual void Free()
    {
        isUpdate = false;
    }

}
