using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehaviour : MonoBehaviour
{

    private bool faded = false;

    public float duration = 0.4f;

    public bool destoryAfterFade = false;

    public void Faded()
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, faded ? 1 : 0));

        faded = !faded;
    }

    private IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }

        if(destoryAfterFade)
            Destroy(transform.parent.gameObject);
    }
}
