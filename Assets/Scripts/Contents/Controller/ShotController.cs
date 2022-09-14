using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField]
    private Transform shotPoint;

    [SerializeField]
    private GameObject projectilePrefab;

    public void Shot() { 
        var go = Instantiate(projectilePrefab);
        var projectileController = go.GetComponent<ProjectileController>();

        projectileController.Shot(shotPoint.position, shotPoint.forward);
    }

}
