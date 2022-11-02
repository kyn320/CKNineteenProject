using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiritWeaponSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject targetObject;

    [SerializeField]
    float spriteDistance = 0.2f;

    [SerializeField]
    float turnTime = 0.75f;
    float turnTimer = 0;

    [SerializeField]
    float moveSpeed = 1000;

    SpiritMoveController spiritMoveController;

    private void Awake()
    {
        spiritMoveController = GetComponent<SpiritMoveController>();
    }

    public void WeaponSpawnMove()
    {
        if (spiritMoveController)
            spiritMoveController.enabled = false;

        StartCoroutine(TurnMove());
    }

    public IEnumerator TurnMove()
    {
        float deg = 0;

        while (true)
        {
            if (turnTime > turnTimer)
            {
                yield return 0;
                turnTimer += Time.deltaTime;
                deg += Time.deltaTime * moveSpeed;

                if (deg < 360)
                {
                    var rad = Mathf.Deg2Rad * (deg);
                    var x = spriteDistance * Mathf.Sin(rad);
                    var y = spriteDistance * Mathf.Cos(rad);
                    Debug.Log($"x : {x} / y : {y} \ndeg : {deg}");
                    transform.position = targetObject.transform.position + new Vector3(x, y, 0);
                }
                else
                    deg = 0;
            }
            else
            {
                if(spiritMoveController)
                spiritMoveController.enabled = true;

                turnTimer = 0;
                yield break;
            }
        }
    }
}
