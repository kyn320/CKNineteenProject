using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;


public class FadeController : Singleton<FadeController>
{
    private CanvasGroup canvasGroup;

    [SerializeField]
    protected List<UITweenAnimator> fadeInAnimatorList;
    [SerializeField]
    protected List<UITweenAnimator> fadeOutAnimatorList;

    [SerializeField]
    private int currentAnimationPlayCount = 0;
    private Coroutine animationCoroutine;

    [SerializeField]
    bool isFadeRunning = false;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetActive(bool isActive) { 
        gameObject.SetActive(isActive);
    }

    [Button("Fade In")]
    public void FadeIn(UnityAction fadeEndAction = null)
    {
        if(isFadeRunning)
            return;

        canvasGroup.blocksRaycasts = true;
        isFadeRunning = true;
        currentAnimationPlayCount = fadeInAnimatorList.Count;

        for (var i = 0; i < fadeInAnimatorList.Count; ++i)
        {
            fadeInAnimatorList[i].PlayAnimation(() =>
            {
                --currentAnimationPlayCount;
            });
        }

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(CoWaitCompleteAnimation(fadeEndAction));
    }

    [Button("Fade Out")]
    public void FadeOut(UnityAction fadeEndAction = null)
    {
        if (isFadeRunning)
            return;

        canvasGroup.blocksRaycasts = true;
        isFadeRunning = true;
        currentAnimationPlayCount = fadeOutAnimatorList.Count;

        for (var i = 0; i < fadeOutAnimatorList.Count; ++i)
        {
            fadeOutAnimatorList[i].PlayAnimation(() =>
            {
                --currentAnimationPlayCount;
            });
        }

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(CoWaitCompleteAnimation(fadeEndAction));
    }

    private IEnumerator CoWaitCompleteAnimation(UnityAction completeEvent)
    {
        while (currentAnimationPlayCount > 0)
        {
            yield return null;
        }

        canvasGroup.blocksRaycasts = false;
        isFadeRunning = false;
        completeEvent?.Invoke();
        animationCoroutine = null;
    }

}
