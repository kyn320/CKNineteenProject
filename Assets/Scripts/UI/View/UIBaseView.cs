using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIBaseView : MonoBehaviour
{
    public string viewName = "";

    public List<UIAnimationData> openAnimationList;
    public List<UIAnimationData> closeAnimationList;

    private List<Tween> playTweenList = new List<Tween>();

    public UnityEvent openEvent;
    public UnityEvent closeEvent;

    protected virtual void Start() { 
        UIController.Instance.OpenView(this);
    }

    public abstract void Init(UIData uiData);

    [Button("Open")]
    public virtual void Open()
    {
        gameObject.SetActive(true);
        PlayAnimation(openAnimationList, EndOpen);
    }

    public virtual void EndOpen()
    {
        openEvent?.Invoke();
    }

    [Button("Close")]
    public virtual void Close()
    {
        closeEvent?.Invoke();
        PlayAnimation(closeAnimationList, EndClose);
    }

    public virtual void EndClose()
    {
        gameObject.SetActive(false);
    }

    public virtual void PlayAnimation(List<UIAnimationData> animations, UnityAction completeEvent = null)
    {
        if (playTweenList.Count > 0)
        {
            Stop();
        }

        for (var i = 0; i < animations.Count; ++i)
        {
            var animationData = animations[i];
            Tween tween = null;
            switch (animationData.AnimationType)
            {
                case UIAnimationType.Move:
                    tween = transform.DOLocalMove(animationData.DestinationVector, animationData.Duration);
                    break;
                case UIAnimationType.Rotate:
                    tween = transform.DOLocalRotate(animationData.DestinationVector, animationData.Duration);
                    break;
                case UIAnimationType.Scale:
                    tween = transform.DOScale(animationData.DestinationVector, animationData.Duration);
                    break;
                case UIAnimationType.Color:
                    tween = GetComponent<Graphic>()?.DOColor(animationData.DestinationColor, animationData.Duration);
                    break;
                case UIAnimationType.Alpha:
                    tween = GetComponent<CanvasGroup>()?.DOFade(animationData.DestinationFloat, animationData.Duration);
                    break;
                case UIAnimationType.ShakePosition:
                    tween = transform.DOShakePosition(animationData.Duration, animationData.Strength, animationData.Vibrato, animationData.Randomness);
                    break;
                case UIAnimationType.ShakeRotation:
                    tween = transform.DOShakeRotation(animationData.Duration, animationData.Strength, animationData.Vibrato, animationData.Randomness);
                    break;
            }

            if (animationData.LoopCount > 0)
            {
                tween.SetLoops(animationData.LoopCount, animationData.LoopType);
            }

            playTweenList.Add(tween);
            tween.SetDelay(animationData.Delay);
            tween.SetEase(animationData.EaseType);
            tween.OnComplete(() =>
            {
                playTweenList.Remove(tween);
                if (playTweenList.Count <= 0)
                {
                    completeEvent?.Invoke();
                }
            });
            tween.SetRelative(animationData.IsRelative);
            tween.Play();
        }
    }

    public void Stop()
    {
        for (var i = 0; i < playTweenList.Count; ++i)
        {
            playTweenList[i].Kill();
        }

        playTweenList.Clear();
    }


}
