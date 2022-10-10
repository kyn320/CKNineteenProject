using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField]
    private List<string> ignoreTags;

    public UnityEvent<GameObject> enableEvent;
    public UnityEvent<GameObject> disableEvent;

    public UnityEvent<Collision> collisionEnterEvent;
    public UnityEvent<Collision> collisionStayEvent;
    public UnityEvent<Collision> collisionExitEvent;

    public UnityEvent<Collider> triggerEnterEvent;
    public UnityEvent<Collider> triggerStayEvent;
    public UnityEvent<Collider> triggerExitEvent;

    private void OnEnable()
    {
        enableEvent?.Invoke(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(ignoreTags.Count > 0 && ignoreTags.Contains(collision.gameObject.tag))
            return;

        collisionEnterEvent?.Invoke(collision);
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (ignoreTags.Count > 0 && ignoreTags.Contains(collision.gameObject.tag))
            return;

        collisionStayEvent?.Invoke(collision);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (ignoreTags.Count > 0 && ignoreTags.Contains(collision.gameObject.tag))
            return;

        collisionExitEvent?.Invoke(collision);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (ignoreTags.Count > 0 && ignoreTags.Contains(other.gameObject.tag))
            return;

        triggerEnterEvent?.Invoke(other);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (ignoreTags.Count > 0 && ignoreTags.Contains(other.gameObject.tag))
            return;

        triggerStayEvent?.Invoke(other);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (ignoreTags.Count > 0 && ignoreTags.Contains(other.gameObject.tag))
            return;

        triggerExitEvent?.Invoke(other);
    }
    private void OnDisable()
    {
        disableEvent?.Invoke(gameObject);
    }
}

