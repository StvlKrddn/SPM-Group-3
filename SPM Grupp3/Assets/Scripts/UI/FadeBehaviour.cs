using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBehaviour : MonoBehaviour
{

    private bool faded = false;

    private IEnumerator currentOperation;

    public float duration = 0.4f;

    public bool destroyAfterFade = false;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup.alpha < 1)
            faded = true;
        else
            faded = false;

    }

    public void Fade()
    {
        if (currentOperation != null)
            StopCoroutine(currentOperation);

        currentOperation = DoFade(canvasGroup.alpha, faded ? 1 : 0);

        StartCoroutine(currentOperation);

        faded = !faded;
    }

    private IEnumerator DoFade(float start, float end)
    {
        float counter = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);

                yield return null;
            }
        
        if (destroyAfterFade)
            Destroy(transform.parent.gameObject);
    }

    public bool Faded()
    {
        return faded;
    }
}
