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
    private Coroutine animationCoroutine;

    public UnityEvent openEvent;
    public UnityEvent closeEvent;

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
        for(var i = 0; i < playTweenList.Count; ++i) {
            playTweenList[i].Kill(true);
        }

        playTweenList.Clear();

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
            }

            if (animationData.LoopCount > 0)
            {
                tween.SetLoops(animationData.LoopCount, animationData.LoopType);
            }

            playTweenList.Add(tween);
            tween.SetDelay(animationData.Delay);
            tween.SetEase(animationData.EaseType);
            tween.OnComplete(() => {
                playTweenList.Remove(tween);
            });
            tween.SetRelative(animationData.IsRelative);
            tween.Play();
        }

        animationCoroutine = StartCoroutine("CoWaitCompleteAnimation", completeEvent);
    }

    private IEnumerator CoWaitCompleteAnimation(UnityAction completeEvent)
    {
        while (playTweenList.Count > 0)
        {
            yield return null;
        }
        completeEvent?.Invoke();
        animationCoroutine = null;
    }

}
