using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISettingPopup : UIBasePopup
{
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider mouseSensitivitysSlider;
    [SerializeField]
    private UIBaseText mouseSensitivityText;

    [SerializeField]
    private UISettingPopupData popupData;

    public override void Init(UIData uiData)
    {
        popupData = uiData as UISettingPopupData;

        bgmSlider.SetValueWithoutNotify(SoundManager.Instance.BGMMasterVolume);
        sfxSlider.SetValueWithoutNotify(SoundManager.Instance.SFXMasterVolume);

        mouseSensitivitysSlider.SetValueWithoutNotify(GlobalSetting.Instance.mouseSensitivity);

        mouseSensitivityText.SetText(GlobalSetting.Instance.mouseSensitivity.ToString());
    }

    public void ChangeBGMVolume(float amount)
    {
        SoundManager.Instance.ChangeBGMVolume(amount);
        SaveLoadSystem.Instance.UpdateBGMVolume(amount);
    }

    public void ChangeSFXVolume(float amount)
    {
        SoundManager.Instance.ChangeSFXVolume(amount);
        SaveLoadSystem.Instance.UpdateSFXVolume(amount);
    }

    public void ChangeMouseSensitivity(float amount)
    {
        var amountInt = (int)amount;

        GlobalSetting.Instance.mouseSensitivity = amountInt;
        SaveLoadSystem.Instance.UpdateMouseSensitivity(amountInt);

        mouseSensitivityText.SetText(amountInt.ToString());
    }

    public override void BeginClose()
    {
        Time.timeScale = 1f;
        SaveLoadSystem.Instance.Save();
        base.BeginClose();
    }

    public override void EndClose()
    {
        base.EndClose();
        popupData.endCloseEvent?.Invoke();
    }


}
