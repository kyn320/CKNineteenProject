using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;


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

        [SerializeField]
        private float shieldRadius = 20f;
        [ShowInInspector]
        [ReadOnly]
        private List<Transform> shieldTargetPoints = new List<Transform>();

        [SerializeField]
        private float alertRadius = 100f;

        private void Start()
        {
            ChangeState(LandmarkStateType.LANDMARK_WAIT);
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
            status.OnDamage(damageInfo);
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

        public Transform CreateTargetPoint(Transform directionTarget)
        {
            var newTargetPoint = Instantiate(new GameObject("Landmark_ShieldTarget"), transform);
            var targetPosition = transform.position + (directionTarget.position - transform.position).normalized * shieldRadius;
            targetPosition.y = transform.position.y;

            newTargetPoint.transform.position = targetPosition;
            shieldTargetPoints.Add(newTargetPoint.transform);

            return newTargetPoint.transform;
        }

        public bool CheckInAlertArea(Transform taget)
        {
            return (transform.position - taget.position).sqrMagnitude <= alertRadius * alertRadius;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, shieldRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, alertRadius);

            if (shieldTargetPoints.Count < 1)
                return;

            Gizmos.color = Color.red;
            for (var i = 0; i < shieldTargetPoints.Count; ++i)
            {
                Gizmos.DrawSphere(shieldTargetPoints[i].position, 0.5f);
            }
        }

    }
}
