using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Landmark
{
    public class LandmarkSystem : MonoBehaviour
    {
        [SerializeField]
        private LandmarkState currentState;
        public Dictionary<LandmarkState, LandmarkStateBase> states = new Dictionary<LandmarkState, LandmarkStateBase>();

        [Space(10)]
        public GameObject objPigure = null;
        public  GameObject objCallBox = null;
        public GameObject objField = null;

        [System.NonSerialized]
        public Canvas canvasLandmark = null;

        [Space(10)]
        [Header("Landmark Option")]
        public float maxHp = .0f;
        [System.NonSerialized]
        public float currentHp = .0f;

        [Space(10)]
        [Header("CallBox Option")]
        public GameObject prefabsInteractionText = null;
        [System.NonSerialized]
        public GameObject objInteractionText = null;
        [System.NonSerialized]
        public RectTransform rectTransformInteractionText = null;

        [Space(10)]
        public float callBoxRadius = .0f;
        public float interactionTextOffset = .0f;
        [System.NonSerialized]
        public Collider[] coll;
        [System.NonSerialized]
        public int collCount = 0;

        [Space(10)]
        [Header("HpBar Option")]
        public GameObject prefabsHpBar = null;

        [System.NonSerialized]
        public GameObject objHpBar = null;
        [System.NonSerialized]
        public RectTransform rectTransformHpBar = null;
        [System.NonSerialized]
        public Slider sliderHpBar = null;

        [Space(10)]
        [System.NonSerialized]
        public float hpBarVolume = .0f;
        public float hpBarOffset = .0f;

        [Space(10)]
        public float addHpTimerMaxCount = .0f;
        [System.NonSerialized]
        public float addHpTimerCount = .0f;

        public float addHpVolume = .0f;

        [Space(10)]
        [Header("Field Option")]
        public bool isOnField = false;

        [Space(10)]
        public int fieldRadius = 0;

        public float fieldMaxHp = .0f;
        [System.NonSerialized]
        public float fieldCurrentHp = .0f;

        private void Awake()
        {
            if (prefabsInteractionText == null)
            {
                Debug.LogError("ObjInteractionText is Not Found");
            }

            objPigure = transform.Find("Figure").gameObject;
            canvasLandmark = GameObject.Find("Landmark_Canvas").GetComponent<Canvas>();
            objCallBox = transform.Find("CallBox").gameObject;
            objField = transform.Find("Field").gameObject;

            InitializeStates();
            fieldCurrentHp = fieldMaxHp;

            SetState(LandmarkState.LANDMARK_WAIT);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                SetState(LandmarkState.LANDMARK_READY);
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                SetDamage(10f);
            }
        }

        private void InitializeStates()
        {
            states.Add(LandmarkState.LANDMARK_WAIT, GetComponent<LandmarkWaitState>());
            states.Add(LandmarkState.LANDMARK_READY, GetComponent<LandmarkReadyState>());
            states.Add(LandmarkState.LANDMARK_WORK, GetComponent<LandmarkWorkState>());
            states.Add(LandmarkState.LANDMARK_DESTROY, GetComponent<LandmarkDestroyState>());
        }


        public void SetState(LandmarkState state)
        {
            foreach (var temp in states.Values)
            {
                temp.enabled = false;
            }

            currentState = state;

            states[currentState].enabled = true;
            states[currentState].Action();

            Debug.Log("Change Monster State : " + state.ToString());
        }

        public LandmarkState GetState()
        {
            return currentState;
        }

        public void SetDamage(float damage)
        {
            if (isOnField)
                SetFieldDamage(damage);
            else
                SetLandmarkDamage(damage);
        }

        public void SetLandmarkDamage(float damage)
        {
            if (currentState == LandmarkState.LANDMARK_READY ||
                currentState == LandmarkState.LANDMARK_WORK)
            {
                if (!isOnField)
                {
                    currentHp -= damage;

                    Debug.Log("Current HP : " + currentHp);

                    if (currentHp <= 0)
                        SetState(LandmarkState.LANDMARK_DESTROY);
                }
                else
                {
                    Debug.Log("보호막이 켜져 있습니다. 랜드마크 직접 타격 불가");
                }
            }
        }
        public void SetFieldDamage(float damage)
        {
            if (currentState == LandmarkState.LANDMARK_WORK)
            {
                if (isOnField)
                {
                    fieldCurrentHp -= damage;
                    Debug.Log("Current Field Damaage : " +fieldCurrentHp);

                    if (fieldCurrentHp <= 0)
                    {
                        DestroyField();
                    }
                }
            }
        }

        private void DestroyField()
        {
            if (fieldCurrentHp <= 0 && objField != null)
            {
                isOnField = false;
                Destroy(objField);
            }
        }


        public void SetHpBarPos(Vector3 pos)
        {
            rectTransformHpBar.position = pos;
        }

        public Vector3 CalcHpBarPos()
        {
            var objPos = Camera.main.WorldToScreenPoint(transform.localPosition);
            objPos.y += hpBarOffset;

            return objPos;
        }

        public void CalcHp()
        {
            hpBarVolume = Mathf.Lerp(hpBarVolume, (currentHp / maxHp), Time.deltaTime);
            sliderHpBar.value = hpBarVolume;
        }

        public void OnActive()
        {
            if (currentState == LandmarkState.LANDMARK_READY)
            {
                SetState(LandmarkState.LANDMARK_READY);
            }
        }
    }
}
