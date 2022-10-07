using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    public SerializableDictionary<IKTarget, IKInfo> ikInfoDic = new SerializableDictionary<IKTarget, IKInfo>();

    private Animator animator;

    public bool isActiveIK = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator != null)
            return;

        foreach (var ikInfo in ikInfoDic.Values)
        {
            ikInfo.UpdateIK(animator, isActiveIK);
        }
    }

}
