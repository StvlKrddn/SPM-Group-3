using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBehaviour : MonoBehaviour
{

    private bool faded = false;
    private bool startedFaded = false;
    [SerializeField] private bool looping = false;

    public float duration = 0.4f;

    public bool destoryAfterFade = false;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if(canvasGroup.alpha < 1)
            faded = true;
        else
            faded = false;

    }

    public void Fade()
    {
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
        
        if (destoryAfterFade)
            Destroy(transform.parent.gameObject);
    }

    public bool Faded()
    {
        return faded;
    }
}
