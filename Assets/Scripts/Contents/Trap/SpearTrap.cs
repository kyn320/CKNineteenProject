using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    public enum State
    {
        View,
        ViewWork,
        Hide,
        HideWork,
    }

    [SerializeField]
    private GameObject spearObject;

    [SerializeField]
    private State state;

    [SerializeField]
    private float currentTime;

    [SerializeField]
    private float viewTime;
    [SerializeField]
    private float viewDistance;

    [SerializeField]
    private float hideTime;
    [SerializeField]
    private float hideDistance;

    [SerializeField]
    private float showTime;
    [SerializeField]
    private AnimationCurve animationCurve;

    private void Update()
    {
        currentTime += Time.deltaTime;
        var lerpTime = 0f;
        var attackDirection = Vector3.up;

        switch (state)
        {
            case State.View:
                {
                    lerpTime = currentTime / viewTime;
                    spearObject.transform.localPosition = attackDirection * viewDistance;

                    if (lerpTime >= 1f)
                    {
                        ChangeState(State.HideWork);
                    }
                }
                break;
            case State.ViewWork:
                {
                    lerpTime = currentTime / showTime;

                    spearObject.transform.localPosition = Vector3.Lerp(attackDirection * hideDistance, attackDirection * viewDistance, animationCurve.Evaluate(lerpTime));
                    if (lerpTime >= 1f)
                    {
                        ChangeState(State.View);
                    }
                }
                break;
            case State.Hide:
                {
                    lerpTime = currentTime / hideTime;
                    spearObject.transform.localPosition = attackDirection * hideDistance;
                    if (lerpTime >= 1f)
                    {
                        ChangeState(State.ViewWork);
                    }
                }
                break;
            case State.HideWork:
                {
                    lerpTime = currentTime / showTime;
                    spearObject.transform.localPosition = Vector3.Lerp(attackDirection * viewDistance, attackDirection * hideDistance, animationCurve.Evaluate(lerpTime));
                    if (lerpTime >= 1f)
                    {
                        ChangeState(State.Hide);
                    }
                }
                break;
        }
    }

    public void ChangeState(State state)
    {
        currentTime = 0f;
        this.state = state;
    }

}
