using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    public SerializableDictionary<string, UnityEvent> eventDic;

    public void Emit(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName]?.Invoke();
        }
    }

}
