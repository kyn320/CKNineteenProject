using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Landmark
{
    public class LandmarkReadyState : LandmarkStateBase
    {
        [SerializeField]
        private float currentRecoverTime;

        public float autoRecoverTime;

        private LandmarkStatus status;

        public override void Enter()
        {
            //TODO :: HP 체력 게이지 생성 및 체력 회복 시작.
            //기본 30% 부터 100% 까지 회복.
            //회복 완료 시 WorkState 로 이동
            currentRecoverTime = autoRecoverTime;
            status = controller.GetStatus();
            status.updateHpEvent.AddListener(UpdateHP);
            isStay = true;

            var view = UIController.Instance.OpenView(controller.uiLandmarkViewData);
            controller.uiLandmarkView = view.GetComponent<UILandmarkView>();
            controller.GetStatus().updateHpEvent.AddListener(controller.uiLandmarkView.UpdateShieldAmount);
            controller.GetStatus().ForceUpdateHPEvent();
            WorldController.Instance.AlertActiveLandmark(controller);

            enterEvent?.Invoke();
        }

        public override void Exit()
        {
            status.updateHpEvent.RemoveListener(UpdateHP);

            isStay = false;
            exitEvent?.Invoke();
        }

        public override void Update()
        {
            currentRecoverTime -= Time.deltaTime;

            if (currentRecoverTime <= 0)
            {
                currentRecoverTime = autoRecoverTime;
                status.AutoRecover();
            }
        }

        public void UpdateHP(float current, float max)
        {
            if (current >= max)
            {
                //TODO :: 랜드마크 보호막 작동 시작.
                controller.ChangeState(LandmarkStateType.LANDMARK_WORK);
            }
        }
    }
}