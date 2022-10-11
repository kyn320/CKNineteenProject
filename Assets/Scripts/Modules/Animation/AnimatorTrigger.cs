using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    protected List<AnimatorTriggerData> animatorTriggerList;


    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(int index)
    {
        if (animatorTriggerList.Count < 1 && index > animatorTriggerList.Count - 1)
            return;

        animatorTriggerList[index].Invoke(animator);

    }

}
