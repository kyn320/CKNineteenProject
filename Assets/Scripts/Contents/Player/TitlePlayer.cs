using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public AmountRangeFloat timeRange;

    [SerializeField]
    private float animationTime;

    [SerializeField]
    private float currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= animationTime)
        {
            animator.SetTrigger("Special");
            currentTime = 0f;
            animationTime = timeRange.GetRandomAmount();
        }
    }
}
