using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComboPanel : MonoBehaviour
{
    [SerializeField]
    private UIAmountText hitComboAmountText;

    [SerializeField]
    private UIAmountText killComboAmountText;

    public void UpdateHitCombo(int combo)
    {
        hitComboAmountText.UpdateAmount(combo);
    }

    public void UpdateKillCombo(int combo)
    {
        killComboAmountText.UpdateAmount(combo);
    }


}
