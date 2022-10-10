using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkDestroyState : LandmarkStateBase
    {
        public override void Enter()
        {
            //TODO :: ���帶ũ �ı� �� ������� ����
            //�׽�Ʈ ���� �Ⱓ���� Active�� ����
            controller.GetStatus().updateHpEvent.RemoveListener(controller.uiLandmarkView.UpdateShieldAmount);
            UIController.Instance.CloseView(controller.uiLandmarkView);
            WorldController.Instance.AlertActiveLandmark(null);
            isStay = true;
            enterEvent?.Invoke();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            return;
        }
    }
}