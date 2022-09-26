using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackHitBox : MonoBehaviour
{
    public string targetTag;

    public UnityEvent<GameObject> enableEvent;
    public UnityEvent<GameObject> disableEvent;

    public UnityEvent<Collision, Vector3> collisionEnterEvent;
    public UnityEvent<Collision, Vector3> collisionStayEvent;
    public UnityEvent<Collision, Vector3> collisionExitEvent;

    public UnityEvent<Collider, Vector3> triggerEnterEvent;
    public UnityEvent<Collider, Vector3> triggerStayEvent;
    public UnityEvent<Collider, Vector3> triggerExitEvent;

    public void ChangeTargetTag(string tagName) { 
        targetTag = tagName;
    }

    private void OnEnable()
    {
        enableEvent?.Invoke(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag(targetTag))
            return;

        collisionEnterEvent?.Invoke(collision, collision.GetContact(0).point);
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
            return;

        collisionStayEvent?.Invoke(collision, collision.GetContact(0).point);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
            return;

        collisionExitEvent?.Invoke(collision, collision.GetContact(0).point);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(targetTag))
            return;

        triggerEnterEvent?.Invoke(other, other.ClosestPoint(transform.position));
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag(targetTag))
            return;

        triggerStayEvent?.Invoke(other, other.ClosestPoint(transform.position));
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(targetTag))
            return;

        triggerExitEvent?.Invoke(other, other.ClosestPoint(transform.position));
    }
    private void OnDisable()
    {
        disableEvent?.Invoke(gameObject);
    }
}

