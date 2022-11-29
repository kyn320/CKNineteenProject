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
    protected State initState;

    [SerializeField]
    [ReadOnly]
    protected State state = State.Close;

    [SerializeField]
    private ObjectTweenAnimator openAnimator;
    public UnityEvent<Door> beginOpenEvent;
    public UnityEvent<Door> endOpenEvent;

    [SerializeField]
    private ObjectTweenAnimator closeAnimator;
    public UnityEvent<Door> beginCloseEvent;
    public UnityEvent<Door> endCloseEvent;

    protected virtual void Start()
    {
        if (initState != state)
        {
            switch (initState)
            {
                case State.Open:
                    Open();
                    break;
                case State.Close:
                    Close();
                    break;
            }
        }
    }

    [Button("Open")]
    public virtual void Open()
    {
        if (state == State.Open || state == State.OpenWork)
            return;

        state = State.OpenWork;
        beginOpenEvent?.Invoke(this);

        openAnimator.PlayAnimation(() =>
        {
            endOpenEvent?.Invoke(this);
            state = State.Open;
        });
    }

    [Button("Close")]
    public virtual void Close()
    {
        if (state == State.Close || state == State.CloseWork)
            return;

        state = State.CloseWork;
        beginCloseEvent?.Invoke(this);

        closeAnimator.PlayAnimation(() =>
        {
            endCloseEvent?.Invoke(this);
            state = State.Close;
        });
    }
}
