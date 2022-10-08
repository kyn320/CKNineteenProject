using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkStatus : UnitStatus
    {
        public void ForceUpdateHPEvent()
        {
            var hpElement = currentStatus.GetElement(StatusType.HP);
            var maxHPElement = currentStatus.GetElement(StatusType.MaxHP);
            updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        }

        public void SetHP(float amount, bool isPercent = false)
        {
            var hpElement = currentStatus.GetElement(StatusType.HP);
            var maxHPElement = currentStatus.GetElement(StatusType.MaxHP);

            if (isPercent)
            {
                hpElement.SetAmount(maxHPElement.CalculateTotalAmount() * amount * 0.01f);
            }
            else
                hpElement.SetAmount(amount);

            updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        }

        public void AddHP(float amount, bool isPercent = false)
        {
            var hpElement = currentStatus.GetElement(StatusType.HP);
            var maxHPElement = currentStatus.GetElement(StatusType.MaxHP);

            if (isPercent)
            {
                hpElement.AddAmount(maxHPElement.CalculateTotalAmount() * amount * 0.01f);
            }
            else
                hpElement.AddAmount(amount);

            updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        }

        public void AutoRecover()
        {
            var autoRecoverElement = currentStatus.GetElement(StatusType.RecoverHP);
            AddHP(autoRecoverElement.GetPercent(), true);
        }
    }
}