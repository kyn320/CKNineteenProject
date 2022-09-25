using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Landmark
{
    public class LandmarkSystem : MonoBehaviour
    {
        [SerializeField]
        private Landmark_State currentState;

        [Space(10)]
        [SerializeField]
        private GameObject objPigure = null;
        [SerializeField]
        private GameObject objCallBox = null;
        [SerializeField]
        private GameObject objField = null;

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

        [Space(10)]
        [Header("Field Option")]
        [SerializeField]
        private bool isOnField = false;

        [Space(10)]
        [SerializeField]
        private int fieldRadius = 0;

        [SerializeField]
        private float fieldMaxHp = .0f;
        private float fieldCurrentHp = .0f;



        #region LifeCycle
        private void Awake()
        {
            if (prefabsInteractionText == null)
            { 
                Debug.LogError("ObjInteractionText is Not Found"); 
            }

            objPigure = transform.Find("Figure").gameObject;

            canvasLandmark = GameObject.Find("Landmark_Canvas").GetComponent<Canvas>();

            objField = transform.Find("Field").gameObject;

            SetState(Landmark_State.LANDMARK_WAIT);

            fieldCurrentHp = fieldMaxHp;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //    SetState((Landmark_State)0);
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //    SetState((Landmark_State)1);
            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //    SetState((Landmark_State)2);
            //if (Input.GetKeyDown(KeyCode.Alpha4))
            //    SetState((Landmark_State)3);

            //if (Input.GetKeyDown(KeyCode.R))
            //    SetLandmarkDamage(5f);
            //if (Input.GetKeyDown(KeyCode.T))
            //    SetFieldDamage(2f);


            switch(currentState)
            {
                case Landmark_State.LANDMARK_WAIT:
                    CreatePlayerSearchCircle(callBoxRadius);

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
                    if(objHpBar != null)
                    {
                        SetHpBarPos(CalcHpBarPos());
                        CalcHp();
                    }
                    break;
                case Landmark_State.LANDMARK_DESTROY:
                    break;
            }

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
            CreateField();
        }

        private void DestroyInitialize()
        {
            Destroy(objHpBar);

            // Destroy 애니메이션 추가해주면 됌.
        }
        #endregion

        // Landmark State의 변수에 영향을 끼치는 Method Region
        #region StateManager

        public void SetLandmarkDamage(float damage)
        {
            if ( currentState == Landmark_State.LANDMARK_READY ||
                currentState == Landmark_State.LANDMARK_WORK )
            {
                if (!isOnField)
                {
                    currentHp -= damage;

                    Debug.Log("Current HP : " + currentHp);

                    if(currentHp <= 0)
                        SetState(Landmark_State.LANDMARK_DESTROY);
                } else
                {
                    Debug.Log("보호막이 켜져 있습니다. 랜드마크 직접 타격 불가");
                }
            }
        }

        public void SetFieldDamage(float damage)
        {
            if (currentState == Landmark_State.LANDMARK_WORK)
            {
                if (isOnField)
                {
                    fieldCurrentHp -= damage;
                    Debug.Log("Current Field Damaage : " + fieldCurrentHp);

                    if (fieldCurrentHp <= 0)
                    {
                        DestroyField();
                    }
                }
            }
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

        private void CalcHp()
        {
            hpBarVolume = Mathf.Lerp(hpBarVolume, (currentHp / maxHp), Time.deltaTime);
            sliderHpBar.value = hpBarVolume;
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

       public void OnActive()
        {
            if(currentState == Landmark_State.LANDMARK_READY)
            {
                OnReadyState();
            }
        }

        private void OnReadyState()
        {
            SetState(Landmark_State.LANDMARK_READY);
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
                currentHp = maxHp;

                SetState(Landmark_State.LANDMARK_WORK);
            }
        }

        private void AddHp(float volume)
        {
            currentHp += volume;
        }

        #endregion

        #region Work
        private void CreateField()
        {
            objField.SetActive(true);

            StartCoroutine("ScaleField", fieldRadius);
            PlayFieldEffect();

            isOnField = true;
        }

        private IEnumerator ScaleField(float radius)
        {
            var fieldScale = objField.transform.localScale;
            var diameter = radius * 2;

            for(var i = 0; i< diameter; i++)
            {
                objField.transform.localScale = new Vector3(fieldScale.x + i, fieldScale.y + i, fieldScale.z + i);
                PlayFieldEffect();
                yield return new WaitForSeconds(0.5f);
            }

            StopCoroutine("ScaleField");
        }

        private void PlayFieldEffect()
        {
            Collider[] fieldColl = Physics.OverlapSphere(objField.transform.localPosition, fieldRadius);

            for (var i = 0; i < fieldColl.Length; i++)
            {
                if (fieldColl[i].name == "Field") continue;
                Debug.Log(fieldColl[i].name);

                // 필드가 생성되면서 존재하는 몬스터를 여기에서 처리한다.
            }

        }

        private void DestroyField()
        {
           if(fieldCurrentHp <= 0 && objField != null)
            {
                isOnField = false;
                Destroy(objField);
            }
        }

        #endregion 
    }
}
