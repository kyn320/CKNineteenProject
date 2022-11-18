using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnZone : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    public List<string> tagNames;

    public UnityEvent<GameObject> enterZoneEvent;

    public void Spawn(Collider other)
    {
        if (!tagNames.Contains(other.gameObject.tag))
            return;

        other.transform.position = spawnPoint.position;
        enterZoneEvent?.Invoke(other.gameObject);
    }

}
