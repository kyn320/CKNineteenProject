using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public class LandmarkDestroyState : LandmarkStateBase
    {
        public override void Enter()
        {
            //TODO :: 랜드마크 파괴 된 모습으로 변경
            //테스트 빌드 기간에는 Active만 끄기
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