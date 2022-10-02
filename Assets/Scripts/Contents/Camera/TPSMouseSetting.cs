using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMouseSetting : Singleton<TPSMouseSetting>
{
    [SerializeField]
    private bool isOpenUI = false;

    [SerializeField]
    private bool isLocked = false;

    private void Start()
    {
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isLocked)
                LockCursor();
            else
                UnlockCursor();
        }
    }


    public void LockCursor()
    {
        if(isOpenUI)
            return;

        isLocked = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
 
    public void UnlockCursor()
    {
        isLocked = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenUICursor() { 
        isOpenUI = true;
        UnlockCursor();
    }

    public void CloseUICursor()
    {
        isOpenUI = false;
        LockCursor();
    }

}
