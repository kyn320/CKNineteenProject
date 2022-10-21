using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintSplashRender : MonoBehaviour
{
    private SkinnedMeshRenderer[] meshRenderers;
    private MaterialPropertyBlock materialProperty;

    [SerializeField]
    private Color tintColor;
    [SerializeField]
    private float splashTime;

    private Coroutine splashCoroutine;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        materialProperty = new MaterialPropertyBlock();
    }

    public void Splash()
    {
        ChangeColor(tintColor);

        if (splashCoroutine != null)
        {
            StopCoroutine(splashCoroutine);
        }

        splashCoroutine = StartCoroutine(CoSplashAnimation());
    }

    private void ChangeColor(Color color)
    {

        materialProperty.SetColor("_BaseColor", color);

        foreach (var render in meshRenderers)
        {
            render.SetPropertyBlock(materialProperty);
        }
    }

    IEnumerator CoSplashAnimation()
    {
        yield return new WaitForSeconds(splashTime);
        ChangeColor(Color.white);
        splashCoroutine = null;
    }



}
