using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkStatus : UnitStatus
    {
        public bool isSuper = false;

        public void SetSuper(bool isSuper)
        {
            this.isSuper = isSuper;
        }

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

            if (hpElement.GetAmount() >= maxHPElement.GetAmount())
                hpElement.SetAmount(maxHPElement.GetAmount());

            updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        }

        public void AutoRecover()
        {
            var autoRecoverElement = currentStatus.GetElement(StatusType.RecoverHP);
            AddHP(autoRecoverElement.GetPercent(), true);
        }

        public override DamageInfo OnDamage(DamageInfo damageInfo)
        {
            if (isSuper)
            {
                damageInfo.isHit = false;
                damageInfo.isKill = false;
                return damageInfo;
            }

            return base.OnDamage(damageInfo);
        }

    }
}