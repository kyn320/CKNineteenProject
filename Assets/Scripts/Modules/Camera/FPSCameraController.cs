using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    private Vector3 aimPosition;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
        }
    }
    public void Rotate(Vector2 mouseMovePosition)
    {
        aimPosition.x -= mouseMovePosition.y;
        aimPosition.x = Mathf.Clamp(aimPosition.x, -55f, 55f);

        transform.localRotation = Quaternion.Euler(aimPosition.x, 0f, 0f);
    }


}
