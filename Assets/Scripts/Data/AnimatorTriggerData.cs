using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimatorTriggerData
{
    public AnimatorTriggerType triggerType;
    public string parameterName;

    public int valueInt;
    public float valueFloat;
    public bool valueBool;

    public void Invoke(Animator animator)
    {
        switch (triggerType)
        {
            case AnimatorTriggerType.Float:
                animator.SetFloat(parameterName, valueFloat);
                break;
            case AnimatorTriggerType.Int:
                animator.SetInteger(parameterName, valueInt);
                break;
            case AnimatorTriggerType.Bool:
                animator.SetBool(parameterName, valueBool);
                break;
            case AnimatorTriggerType.Trigger:
                animator.SetTrigger(parameterName);
                break;
            default:
                break;
        }
    }

}
