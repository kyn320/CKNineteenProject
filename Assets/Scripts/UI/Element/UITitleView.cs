using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UITitleView : UIBaseView
{
    public bool isEnterTitle = false;

    public UnityEvent enterTitleEvent;

    public override void Init(UIData uiData)
    {

    }

    protected override void Start()
    {
        base.Start();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Update()
    {
        if (isEnterTitle)
            return;

        if (Input.anyKeyDown)
        {
            isEnterTitle = true;
            enterTitleEvent?.Invoke();
        }
    }

    public void EnterGame()
    {
        FadeController.Instance.FadeIn(() => {
            SceneLoader.Instance.SwitchScene("PlayScene");
        });
    }

    public void EnterCreadit() {
        FadeController.Instance.FadeIn(() => {
            SceneLoader.Instance.SwitchDirectScene("CreditScene");
        });
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
