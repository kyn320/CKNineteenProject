using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComboPanel : MonoBehaviour
{
    [SerializeField]
    private UIAmountText hitComboAmountText;

    [SerializeField]
    private UIAmountText killComboAmountText;

    private void Start()
    {
        ComboSystem.Instance.updateHitCombo.AddListener(UpdateHitCombo);
        ComboSystem.Instance.updateKillCombo.AddListener(UpdateKillCombo);
        gameObject.SetActive(false);
    }

    public void UpdateHitCombo(int combo, int maxCombo)
    {
        gameObject.SetActive(true);
        hitComboAmountText.UpdateAmount(combo);
    }

    public void UpdateKillCombo(int combo, int maxCombo)
    {
        gameObject.SetActive(true);
        killComboAmountText.UpdateAmount(combo);
    }


}
