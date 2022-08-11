using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearBehaviour : MonoBehaviour
{
    private float time = 0f;
    [SerializeField] private bool appearOnDefualt = false;
    [SerializeField] private float originalSizeDif;
    [SerializeField] private float duration = 1f;

    private void OnEnable()
    {
        time = 0f;
        originalSizeDif = transform.localScale.x;

        if (appearOnDefualt)
            Appear();
    }

    private IEnumerator currentOperation;
    private float startScale;
    [SerializeField] private bool visible;

    public void Appear()
    {
        if (currentOperation != null)
            StopCoroutine(currentOperation);

        currentOperation = DoAppear();
        visible = true;
        StartCoroutine(currentOperation);
    }

    private IEnumerator DoAppear()
    {
        time = 0;

        if(transform.localScale.x <= 0.1)
            while (time <= duration)
            {
                time += Time.deltaTime;
                transform.localScale = Vector3.one * Mathfx.Berp(0f, originalSizeDif, time / duration);

                yield return null;
            }
        else
        {
            startScale = transform.localScale.x;

            while (time <= duration)
            {
                time += Time.deltaTime;
                transform.localScale = Vector3.one * Mathfx.Berp(startScale, originalSizeDif, time / duration);

                yield return null;
            }
        }
    }

    public void Disappear()
    {
        if (currentOperation != null)
            StopCoroutine(currentOperation);

        currentOperation = DoDisappear();
        visible = false;
        StartCoroutine(currentOperation);
    }

    private IEnumerator DoDisappear()
    {
        time = 0;
        startScale = transform.localScale.x;

        while (time <= duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.one * Mathfx.Berp(startScale, 0, time/duration);

            yield return null;
        }
    }
}
