using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CrowdBehaviour : MonoBehaviour
{
    [SerializeField]
    private BuffController controller;
    [SerializeField]
    protected PlayerController playerController;
    [SerializeField]
    protected MonsterController monsterController;
    public WeaponData weaponData;
    protected GameObject userObject;

    [SerializeField]
    private BuffData buffData;

    [SerializeField]
    public CrowdType crowdType;

    protected bool isActive = false;

    [SerializeField]
    protected float currentLifeTime;

    private int stackCount = 0;

    public GameObject prefabsEffect = null;
    protected GameObject effectObject = null;

    public Vector3 effectPos;

    public UnityEvent<CrowdBehaviour> enterEvent;
    public UnityEvent<CrowdBehaviour, float> updateEvent;
    public UnityEvent<CrowdBehaviour> exitEvent;

    private void Awake()
    {
        userObject = transform.parent.gameObject;
    }

    protected virtual void Update()
    {
        if (!isActive)
            return;
        
        Active();

        currentLifeTime -= Time.deltaTime;
        updateEvent?.Invoke(this, currentLifeTime);

        if (currentLifeTime <= 0f)
        {
            isActive = false;

            exitEvent?.Invoke(this);

            Destroy(gameObject);
            Destroy(effectObject);

            UnActive();
        }
    }

    public void StartBuff()
    {
        if(stackCount > 0)
            ResetCooltime();

        crowdType = buffData.CrowdTypes[0];
        string userTag = transform.parent.tag;

        if (playerController == null)
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if(userTag == "Monster")
            monsterController = controller.GetComponent<MonsterController>();


        effectObject = Instantiate(prefabsEffect, userObject.transform);
        effectObject.transform.localPosition = effectPos;

        isActive = true;

        ApplyCrowd();
    }

    public void ResetCooltime()
    {
        currentLifeTime = buffData.LifeTime;
    }

    // 초기 장착 필요.
    public virtual void SetBuffController(BuffController controller)
    {
        this.controller = controller;
    }

    public virtual void SetBuffData(BuffData buffData)
    {
        this.buffData = buffData;
    }

    public virtual void Active()
    {
    }

    public virtual void UnActive()
    {
    }

    protected virtual void ApplyCrowd()
    {
        enterEvent?.Invoke(this);
    }

    // 남은 시간 안내.
    public float GetLifeTime()
    {
        return currentLifeTime;
    }

    public BuffData GetBuffData()
    {
        return buffData;
    }
}
