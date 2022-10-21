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

    // �������ΰ�?
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
        // Buff�� Active�� �ƴ� ��쿣 ���� �ʴ´�.
        if (!isActive)
            return;

        // Active�� ��, �����ش�.
        Active();

        // ������ ���� �ð� ����.
        currentLifeTime -= Time.deltaTime;

        // currentLifeTime�� �������� ���.
        if (currentLifeTime <= 0f)
        {
            // �ش� �����̻� UnActive
            UnActive();
        }
    }

    public void StartBuff()
    {
        // ���� ���� �ÿ� ���⿡ �־.

        if(stackCount > 0)
        {
            // �ش� ������ ��ø�̹Ƿ�, ��ø ���� ���ش�.
        }

        ApplyCrowd();
        isActive = true;
        // Active�� true�� ���������μ�, Active �޼��嵵 ����.
    }

    // �ʱ� ���� �ʿ�.
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

    // ���� �ð� �ȳ�.
    public float GetLifeTime()
    {
        return currentLifeTime;
    }
}
