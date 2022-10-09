using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace Landmark
{
    public class LandmarkController : MonoBehaviour
    {
        [SerializeField]
        LandmarkStatus status;

        [SerializeField]
        private bool isDebugMode;

        [SerializeField]
        private LandmarkStateType currentStateType;

        [SerializeField]
        private SerializableDictionary<LandmarkStateType, LandmarkStateBase> statesDic
            = new SerializableDictionary<LandmarkStateType, LandmarkStateBase>();

        [HideInInspector]
        public UILandmarkViewData uiLandmarkViewData = new UILandmarkViewData();
        [HideInInspector]
        public UILandmarkView uiLandmarkView;

        public float shieldRadius = 20f;

        public Transform TESTDEBUGMONSTER;

        private void Start()
        {
            ChangeState(LandmarkStateType.LANDMARK_WAIT);
        }

        private void Update()
        {
            if (isDebugMode)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ChangeState(LandmarkStateType.LANDMARK_READY);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    //TODO 강제로 데미지 피해 주기
                }
            }
        }

        public void AddState(LandmarkStateType stateType, LandmarkStateBase state)
        {
            statesDic.Add(stateType, state);
        }

        public void ChangeState(LandmarkStateType state)
        {
            statesDic[currentStateType].Exit();

            foreach (var stateBehaviour in statesDic.Values)
            {
                stateBehaviour.enabled = false;
            }

            currentStateType = state;
            Debug.Log($"Landmark :: ChangeState >> { currentStateType }");

            statesDic[currentStateType].enabled = true;
            statesDic[currentStateType].Enter();
        }

        public LandmarkStateType GetState()
        {
            return currentStateType;
        }

        public LandmarkStatus GetStatus()
        {
            return status;
        }

        public void OnDamage(DamageInfo damageInfo)
        {
            //TODO :: WAIT 인 경우 지형에 충돌 시 데미지 피해
            //TODO :: WORK 인 경우 쉴드 충돌 시 데미지 피해
            status.OnDamage(damageInfo.damage);
        }

        public void Destroy()
        {
            //TODO :: 랜드마크 파괴 처리
            gameObject.SetActive(false);
        }

        public void Interactive()
        {
            if (currentStateType != LandmarkStateType.LANDMARK_WAIT)
                return;

            //TODO :: Ready 상태로 변경하기
            ChangeState(LandmarkStateType.LANDMARK_READY);
        }

        private void OnDrawGizmos()
        {
            if (TESTDEBUGMONSTER == null)
                return;

            Gizmos.color = Color.red;

            var targetPoint = transform.position + (TESTDEBUGMONSTER.position - transform.position).normalized * shieldRadius;
            targetPoint.y = transform.position.y;

            Gizmos.DrawSphere(targetPoint, 0.5f);
        }

    }
}
