using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CrowdBehaviour : MonoBehaviour
{
    [SerializeField]
    private BuffController controller;
    protected PlayerController playerController;

    [SerializeField]
    private BuffData buffData;

    [SerializeField]
    protected CrowdType crowdType;

    public UnityEvent enterEvent, exitEvent, stayEvent;

    // 실행중인가?
    protected bool isActive = false;

    [SerializeField]
    protected float currentLifeTime;

    private int stackCount = 0;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    protected virtual void Update()
    {
        // Buff가 Active가 아닐 경우엔 돌지 않는다.
        if (!isActive)
            return;

        // Active일 땐, 돌려준다.
        Active();

        // 프레임 마다 시간 제외.
        currentLifeTime -= Time.deltaTime;

        // currentLifeTime에 도달했을 경우.
        if (currentLifeTime <= 0f)
        {
            // 해당 상태이상 UnActive
            UnActive();
        }
    }

    public void StartBuff()
    {
        // 버프 시작 시에 여기에 넣어서.

        if(stackCount > 0)
        {
            // 해당 버프는 중첩이므로, 중첩 설정 해준다.
        }

        ApplyCrowd();
        isActive = true;
        // Active를 true로 변경함으로서, Active 메서드도 실행.
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
        stayEvent?.Invoke();
    }

    public virtual void UnActive()
    {
        exitEvent?.Invoke();

    }

    protected virtual void ApplyCrowd()
    {
        enterEvent?.Invoke();

    }

    // 남은 시간 안내.
    public float GetLifeTime()
    {
        return currentLifeTime;
    }
}
