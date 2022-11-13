using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnZone : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    public UnityEvent<GameObject> enterZoneEvent;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = spawnPoint.position;
        other.transform.rotation = spawnPoint.rotation;

        enterZoneEvent?.Invoke(other.gameObject);
    }
}
