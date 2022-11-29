using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMousePointerSFXPlayer : SFXPlayer , IPointerEnterHandler , IPointerDownHandler
{
    [SerializeField]
    protected SerializableDictionary<string, AudioClip> interactionAudioClipDic;

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioClip = interactionAudioClipDic["Hover"];
        Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioClip = interactionAudioClipDic["Down"];
        Play();
    }
}
