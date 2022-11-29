using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Door : MonoBehaviour
{
    public enum State
    {
        Open,
        OpenWork,
        Close,
        CloseWork,
    }

    [SerializeField]
    protected State state;

    [SerializeField]
    private ObjectTweenAnimator openAnimator;
    public UnityEvent<Door> beginOpenEvent;
    public UnityEvent<Door> endOpenEvent;

    [SerializeField]
    private ObjectTweenAnimator closeAnimator;
    public UnityEvent<Door> beginCloseEvent;
    public UnityEvent<Door> endCloseEvent;

    [SerializeField]
    protected Transform interactionTargetObject;

    [SerializeField]
    protected GameObject interactionUIPrefab;

    protected GameObject interactionUIObject;


    protected Collider[] overlapColliders;
    [SerializeField]
    protected Vector3 offset;
    [SerializeField]
    protected float overlapRadius;

    [SerializeField]
    protected LayerMask layerMask;

    private void Start()
    {
        //TODO :: 미리 상호작용 가이드UI 생성하기
        interactionUIObject = UIController.Instance.CreateWorldUI(interactionUIPrefab);
        interactionUIObject.GetComponent<UITargetFollower>().SetTarget(interactionTargetObject);
    }

    public virtual void Update()
    {
        if (state == State.Open || state == State.OpenWork)
        {
            ShowUI(false);
            return;
        }

        //TODO :: 상호작용 가이드 UI 범위내에 들어온 경우 표시 / 숨기기
        overlapColliders = Physics.OverlapSphere(transform.position + offset, overlapRadius, layerMask);
        ShowUI(overlapColliders.Length > 0);
    }

    public void ShowUI(bool isShow)
    {
        interactionUIObject?.SetActive(isShow);
    }

    [Button("Open")]
    public virtual void Open()
    {
        if (state == State.Open || state == State.OpenWork)
            return;

        state = State.OpenWork;
        beginOpenEvent?.Invoke(this);

        openAnimator.PlayAnimation(() =>
        {
            endOpenEvent?.Invoke(this);
            state = State.Open;
        });
    }

    [Button("Close")]
    public virtual void Close()
    {
        if (state == State.Close || state == State.CloseWork)
            return;

        state = State.CloseWork;
        beginCloseEvent?.Invoke(this);

        closeAnimator.PlayAnimation(() =>
        {
            endCloseEvent?.Invoke(this);
            state = State.Close;
        });
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, overlapRadius);
    }
}
