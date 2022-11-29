using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour, IInteractable
{
    public UnityEvent interactionEvent;

    public bool isInteractable = true;

    public void ChangeInteractive(bool isInteractable)
    {
        this.isInteractable = isInteractable;
    }

    public virtual bool Interactive()
    {
        if (!isInteractable)
            return false;

        interactionEvent?.Invoke();
        return true;
    }
}
