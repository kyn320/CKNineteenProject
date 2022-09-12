using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTagChecker : MonoBehaviour
{
    public string tagName;

    public UnityEvent workEvent;

    public void CheckCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagName))
        {
            workEvent?.Invoke();
        }
    }

    public void CheckCollider(Collider collider)
    {
        if (collider.gameObject.CompareTag(tagName))
        {
            workEvent?.Invoke();
        }
    }

}
