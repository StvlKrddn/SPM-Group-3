using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mFadeBehaviour : MonoBehaviour
{
    private bool mFaded = false;

    public float Duration = 0.4f;

    public void Faded()
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, mFaded ? 1 : 0));

        mFaded = !mFaded;
    }

    private IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / Duration);

            yield return null;
        }
    }
}
