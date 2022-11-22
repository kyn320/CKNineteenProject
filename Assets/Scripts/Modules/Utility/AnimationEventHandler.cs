using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class AnimationEventHandler : MonoBehaviour
{
    public SerializableDictionary<string, UnityEvent<int,float>> animatoinEventDic;

    public void Emit(AnimationEvent animationEvent)
    {
        if (animatoinEventDic.ContainsKey(animationEvent.stringParameter))
        {
            animatoinEventDic[animationEvent.stringParameter]?.Invoke(animationEvent.intParameter, animationEvent.floatParameter);
        }
    }

}
