using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehaviour : MonoBehaviour
{

    private bool mFaded = false;
    private Transform lookAt;

    public float Duration = 0.4f;

    public float moveSpeed = 1;

    /*public void mFaded()
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, FadeBehaviour.mFaded ? 1 : 0));

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

        Destroy(transform.parent.gameObject);
    }*/
}
