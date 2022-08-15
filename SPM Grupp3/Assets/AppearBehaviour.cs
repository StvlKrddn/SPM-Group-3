using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearBehaviour : MonoBehaviour
{
    private float time = 0f;
    [SerializeField] private bool appearOnDefualt = false;
    [SerializeField] private float originalSizeDif = 0;
    [SerializeField] private float duration = 1f;

    private CanvasGroup canvasGroup;
    private bool visible;

    private IEnumerator currentOperation;

    private void Awake()
    {
        if(originalSizeDif == 0)
            originalSizeDif = transform.localScale.x;

        if (gameObject.GetComponent<CanvasGroup>() != null)
            canvasGroup = gameObject.GetComponent<CanvasGroup>();

        if (transform.localScale.x > 0)
        {
            visible = true;
            if (canvasGroup != null)
                canvasGroup.alpha = 1;
        }
            
    }

    private void OnEnable()
    {
        time = 0f;

        if (appearOnDefualt)
            Appear();
    }

    private float currentAlpha;

    public void Appear()
    {
        if (visible)
            return;

        visible = true;

        if (currentOperation != null)
            StopCoroutine(currentOperation);

        currentOperation = DoAppear(gameObject.transform.localScale.x);
        StartCoroutine(currentOperation);
    }

    private IEnumerator DoAppear(float startScale)
    {
        time = 0;
        if (canvasGroup != null)
            currentAlpha = canvasGroup.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.one * Mathfx.Berp(startScale, originalSizeDif, time / duration);
            if(canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(currentAlpha, 1, time / duration);

            yield return null;
        }
        gameObject.transform.localScale.Set(originalSizeDif, originalSizeDif, originalSizeDif);
    }

    public void Disappear()
    {
        if (!visible)
            return;

        visible = false;

        if (currentOperation != null)
            StopCoroutine(currentOperation);

        currentOperation = DoDisappear(gameObject.transform.localScale.x);
        StartCoroutine(currentOperation);
    }

    private IEnumerator DoDisappear(float startScale)
    {
        time = 0;

        if (canvasGroup != null)
            currentAlpha = canvasGroup.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.one * Mathfx.Lerp(startScale, 0, time / duration);
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(currentAlpha, 0, time / duration);
            yield return null;
        }

        gameObject.transform.localScale = Vector3.zero;
    }
}
