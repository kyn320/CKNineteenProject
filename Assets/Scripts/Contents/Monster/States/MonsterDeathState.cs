using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : MonsterStateBase
{
    [SerializeField]
    Material deathMaterial;

    [SerializeField]
    SkinnedMeshRenderer[] skinnedMeshRenderers;

    [SerializeField]
    private MaterialPropertyBlock materialProperty;

    [SerializeField]
    private float lifeTime;

    protected override void Awake()
    {
        base.Awake();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        materialProperty = new MaterialPropertyBlock();
    }

    public override void Enter()
    {
        enterEvent?.Invoke();

        for (var i = 0; i < enterAnimatorTriggerList.Count; ++i)
        {
            enterAnimatorTriggerList[i].Invoke(controller.GetAnimator());
        }

        foreach (var meshRenderer in skinnedMeshRenderers)
        {
            meshRenderer.material = deathMaterial;
        }

        StartCoroutine(CoDissolveAnimation());
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        return;
    }

    protected void UpdateDissolve(float t)
    {
        materialProperty.SetFloat("_Diss_Offset", t);

        foreach (var render in skinnedMeshRenderers)
        {
            render.SetPropertyBlock(materialProperty);
        }
    }

    IEnumerator CoDissolveAnimation()
    {
        var currentTime = 0f;
        var lerpTime = currentTime / lifeTime;

        while (lerpTime <= 1f)
        {
            currentTime += Time.deltaTime;
            lerpTime = currentTime / lifeTime;

            UpdateDissolve(1 - lerpTime);
            yield return null;
        }

        Destroy(this.gameObject);
    }

}
