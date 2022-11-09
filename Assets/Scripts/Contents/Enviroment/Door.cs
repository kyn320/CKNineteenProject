using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Door : MonoBehaviour
{
    public enum State
    {
        Open,
        OpenWork,
        Close,
        CloseWork,
    }

    [SerializeField]
    private State state;

    [SerializeField]
    private ObjectTweenAnimator openAnimator;
    public UnityEvent beginOpenEvent;
    public UnityEvent endOpenEvent;

    [SerializeField]
    private ObjectTweenAnimator closeAnimator;
    public UnityEvent beginCloseEvent;
    public UnityEvent endCloseEvent;

    [Button("Open")]
    public void Open()
    {
        state = State.OpenWork;
        beginOpenEvent?.Invoke();

        openAnimator.PlayAnimation(() =>
        {
            endOpenEvent?.Invoke();
            state = State.Open;
        });
    }

    [Button("Close")]
    public void Close()
    {
        state = State.CloseWork;
        beginCloseEvent?.Invoke();

        closeAnimator.PlayAnimation(() =>
        {
            endCloseEvent?.Invoke();
            state = State.Close;
        });
    }

}
