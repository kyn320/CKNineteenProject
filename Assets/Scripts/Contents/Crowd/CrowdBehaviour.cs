using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CrowdBehaviour : MonoBehaviour
{
    public abstract void Active();

    public abstract void UnActive();

    protected abstract void ApplyCrowd();

}
