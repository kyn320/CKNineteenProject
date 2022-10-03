using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour, IInteractable
{
    public UnityEvent interactionEvent;

    public virtual bool Interactive()
    {
        interactionEvent?.Invoke();
        return true;
    }
}
