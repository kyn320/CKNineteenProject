using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : Door
{
    [SerializeField]
    private List<ObjectTweenAnimator> openAnimators;
    [SerializeField]
    private List<ObjectTweenAnimator> closeAnimators;

    private int workAnimationCount;

    public override void Open()
    {
        if(state == State.Open || state == State.OpenWork)
            return;

        state = State.OpenWork;
        beginOpenEvent?.Invoke(this);

        workAnimationCount = openAnimators.Count;

        for (var i = 0; i < workAnimationCount; ++i)
        {
            openAnimators[i].PlayAnimation(() => {
                EndOpen();
            });
        }
    }

    private void EndOpen()
    {
        --workAnimationCount;

        if (workAnimationCount == 0)
        {
            endOpenEvent?.Invoke(this);
            state = State.Open;
        }
    }

    public override void Close()
    {
        if (state == State.Close || state == State.CloseWork)
            return;

        state = State.CloseWork;
        beginCloseEvent?.Invoke(this);

        workAnimationCount = closeAnimators.Count;

        for (var i = 0; i < workAnimationCount; ++i)
        {
            closeAnimators[i].PlayAnimation(() => {
                EndClose();
            });
        }
    }

    private void EndClose()
    {
        --workAnimationCount;

        if (workAnimationCount == 0)
        {
            endCloseEvent?.Invoke(this);
            state = State.Close;
        }
    }

}
