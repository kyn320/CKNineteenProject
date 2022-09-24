using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Landmark
{
    public class LandmarkSystem : MonoBehaviour
    {
        //[SerializeField]
        //private Landmark_State currentState;

        //// 방어막 반지름
        //[SerializeField]
        //private float shieldRadius = .0f;
        //// HP 힐 확률
        //[SerializeField]
        //private int healPercent = 0;
        //// HP 힐 시간
        //[SerializeField]
        //private float healTime = .0f;
        //// HP 힐 양
        //[SerializeField]
        //private int healVolume = 0;

        //[SerializeField]
        //private GameObject prefabShield = null;

        //[Space(10)]

        //[Header("Call Box Settings")]
        //private GameObject callBox = null;

        //[SerializeField]
        //private float callBoxRadius = .0f;

        //[Space(10)]

        //[Header("Landmark Canvas")]
        //private Canvas canvasLandmark = null;

        [SerializeField]
        private Landmark_State currentState;

        [Space(10)]
        [SerializeField]
        private GameObject objPigure = null;
        [SerializeField]
        private GameObject objCallBox = null;

        private Canvas canvasLandmark = null;

        [Space(10)]
        [Header("Landmark Option")]
        [SerializeField]
        private float maxHp = .0f;
        private float currentHp = .0f;

        [Space(10)]
        [Header("CallBox Option")]
        [SerializeField]
        private GameObject prefabsInteractionText = null;
        
        private GameObject objInteractionText = null;
        private RectTransform rectTransformInteractionText = null;

        [Space(10)]
        [SerializeField]
        private float callBoxRadius = .0f;
        [SerializeField]
        private float interactionTextOffset = .0f;
        private Collider[] coll;
        private int collCount = 0;

        [Space(10)]
        [Header("HpBar Option")]
        [SerializeField]
        private GameObject prefabsHpBar = null;

        private GameObject objHpBar = null;
        private RectTransform rectTransformHpBar = null;
        private Slider sliderHpBar = null;

        [Space(10)]
        private float hpBarVolume = .0f;
        [SerializeField]
        private float hpBarOffset = .0f;

        [Space(10)]
        [SerializeField]
        private float addHpTimerMaxCount = .0f;
        private float addHpTimerCount = .0f;

        [SerializeField]
        private float addHpVolume = .0f;



        #region LifeCycle
        private void Awake()
        {
            if (prefabsInteractionText == null)
            { 
                Debug.LogError("ObjInteractionText is Not Found"); 
            }

            objPigure = transform.Find("Figure").gameObject;

            canvasLandmark = GameObject.Find("Landmark_Canvas").GetComponent<Canvas>();

            SetState(Landmark_State.LANDMARK_WAIT);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetState((Landmark_State)0);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SetState((Landmark_State)1);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                SetState((Landmark_State)2);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                SetState((Landmark_State)3);

            switch(currentState)
            {
                case Landmark_State.LANDMARK_WAIT:
                    CreatePlayerSearchCircle(callBoxRadius);
                    Debug.DrawLine(objCallBox.transform.localPosition, Vector3.forward * callBoxRadius, Color.red, callBoxRadius);

                    if (objInteractionText != null)
                    {
                        SetInteractionTextPos(CalcInteractionTextPos());
                    }
                    break;
                case Landmark_State.LANDMARK_READY:
                    if(objHpBar != null)
                    {
                        SetHpBarPos(CalcHpBarPos());
                        CalcHpTimer();

                        CalcHp();
                    }
                    break;
                case Landmark_State.LANDMARK_WORK:
                    break;
                case Landmark_State.LANDMARK_DESTROY:
                    break;
            }

            // Ready
            // Current HP를 MaxHP의 30% 가량으로 설정을 해준다.
            // Monster에게 들어오는 Damage도 존재한다.
            // Hp Bar를 띄워서 User에게 Interface를 제공한다.
            // Current HP는 일정한 N초 마다 일정하게 M씩 상승한다.
            // 그러다가 Current HP가 MaxHP와 같거나 더 높다면, Work로 변경해준다.

            // Work
            // 똑같이 Monster에게 들어오는 Damage가 존재한다.
            // 일정한 Radius만큼 원형 보호막이 생성된다.
            // 보호막 체력은 임의로 설정한다.
            // 보호막이 파괴되었을 경우, Landmark는 데미지를 입는다.
            // 보호막이 생성될 경우, 보호막 Radius 안에 있는 Monster는 사망한다.

            // Destroy
            // Landmark가 사망했을 경우, Player 상호작용 및 아무 것 도 없다~

        }

        #endregion

        // Current State를 관리하는 Method Region
        #region State

        public void SetState(Landmark_State state)
        {
            currentState = state;

            Debug.Log("LandMark State : " + currentState);
            CallStateInitialize(currentState);
        }

        public Landmark_State GetState()
        {
            return currentState;
        }

        #endregion

        // State가 Call 되었을 때, 사용되는 Method Region
        #region Initialize

        private void CallStateInitialize(Landmark_State state)
        {
            switch (state)
            {
                case Landmark_State.LANDMARK_WAIT:
                    WaitInitialize();
                    break;
                case Landmark_State.LANDMARK_READY:
                    ReadyInitialize();
                    break;
                case Landmark_State.LANDMARK_WORK:
                    WorkInitialize();
                    break;
                case Landmark_State.LANDMARK_DESTROY:
                    DestroyInitialize();
                    break;
                default:
                    break;
            }
        }

        private void WaitInitialize()
        {
            objCallBox = transform.Find("CallBox").gameObject;

            if (objInteractionText == null)
            {
                CreateInteractionText();
            }
        }

        private void ReadyInitialize()
        {
            objCallBox = null;

            currentHp = (int)(maxHp * 0.3);
            hpBarVolume = currentHp / maxHp;

            if (objHpBar == null)
            {
                CreateHpBar();
            }
        }

        private void WorkInitialize()
        {
        }

        private void DestroyInitialize()
        {
            // Destroy 
            Debug.Log("랜드마크 파괴");
        }
        #endregion

        // Landmark State의 변수에 영향을 끼치는 Method Region
        #region StateManager
        private void SetDamage(float damage)
        {
            currentHp -= damage;
        }

        #endregion

        // 각 State별로 가지고 있는 Method Region
        #region Wait

        private void CreateInteractionText()
        {
            objInteractionText = Instantiate(prefabsInteractionText);
            objInteractionText.gameObject.SetActive(false);
            
            objInteractionText.transform.SetParent(canvasLandmark.transform);
            rectTransformInteractionText = objInteractionText.GetComponent<RectTransform>();
        }

        private void SetInteractionTextPos(Vector3 pos)
        {
            rectTransformInteractionText.position = pos;
        }

        private Vector3 CalcInteractionTextPos()
        {
            var objPos = Camera.main.WorldToScreenPoint(objCallBox.transform.localPosition);
            objPos.y += interactionTextOffset;

            return objPos;
        }

        private void CreatePlayerSearchCircle(float _radius)
        {
            coll = Physics.OverlapSphere(objCallBox.transform.localPosition, _radius);
            collCount = 0;

            for (var i = 0; i < coll.Length; i++)
            {
                if (coll[i].CompareTag("Player"))
                {
                    OnInteractionText(true);
                    collCount--;
                } else
                {
                    collCount++;
                }

                if (collCount >= coll.Length)
                    OnInteractionText(false);
            }
        }

        private void OnInteractionText(bool trigger)
        {
            objInteractionText.gameObject.SetActive(trigger);
        }

        #endregion

        #region Ready
        private void CreateHpBar()
        {
            objHpBar = Instantiate(prefabsHpBar);

            objHpBar.gameObject.SetActive(false);
            objHpBar.transform.SetParent(canvasLandmark.transform);

            rectTransformHpBar = objHpBar.GetComponent<RectTransform>();
            sliderHpBar = objHpBar.GetComponent<Slider>();

            sliderHpBar.value = hpBarVolume;

            objHpBar.gameObject.SetActive(true);
        }

        private void SetHpBarPos(Vector3 pos)
        {
            rectTransformHpBar.position = pos;
        }

        private Vector3 CalcHpBarPos()
        {
            var objPos = Camera.main.WorldToScreenPoint(transform.localPosition);
            objPos.y += hpBarOffset;

            return objPos;
        }

        private void CalcHpTimer()
        {
            if (currentHp < maxHp)
            {
                addHpTimerCount += Time.deltaTime;

                if (addHpTimerCount > addHpTimerMaxCount)
                {
                    addHpTimerCount = .0f;

                    AddHp(addHpVolume);
                }
            }
            else if(currentHp >= maxHp)
            {
                // On Work
            }
        }

        private void CalcHp()
        {
            hpBarVolume = Mathf.Lerp(hpBarVolume, (currentHp / maxHp), Time.deltaTime);
            sliderHpBar.value = hpBarVolume;
        }

        private void AddHp(float volume)
        {
            currentHp += volume;
        }

        #endregion
    }
}
