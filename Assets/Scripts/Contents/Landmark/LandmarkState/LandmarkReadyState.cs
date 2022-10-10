using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Landmark
{
    public class LandmarkReadyState : LandmarkStateBase
    {
        [SerializeField]
        private GameObject shieldObject;

        [SerializeField]
        private float currentRecoverTime;

        public float autoRecoverTime;

        private LandmarkStatus status;

        public override void Enter()
        {
            //TODO :: HP ü�� ������ ���� �� ü�� ȸ�� ����.
            //�⺻ 30% ���� 100% ���� ȸ��.
            //ȸ�� �Ϸ� �� WorkState �� �̵�
            currentRecoverTime = autoRecoverTime;
            status = controller.GetStatus();
            status.updateHpEvent.AddListener(UpdateHP);
            isStay = true;

            var view = UIController.Instance.OpenView(controller.uiLandmarkViewData);
            controller.uiLandmarkView = view.GetComponent<UILandmarkView>();
            controller.GetStatus().updateHpEvent.AddListener(controller.uiLandmarkView.UpdateShieldAmount);
            controller.GetStatus().ForceUpdateHPEvent();
            shieldObject.SetActive(true);
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
                //TODO :: ���帶ũ ��ȣ�� �۵� ����.
                controller.ChangeState(LandmarkStateType.LANDMARK_WORK);
            }
        }
    }
}