using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBehaviour : MonoBehaviour
{

    private bool faded = false;

    public float duration = 0.4f;

    public bool destroyAfterFade = false;

    private CanvasGroup canvasGroup;
    public bool hover = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if(canvasGroup.alpha < 1)
            faded = true;
        else
            faded = false;

    }

    public void Hover()
    {
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, counter / duration);

        }
        
        if (destroyAfterFade)
            Destroy(transform.parent.gameObject);
    }

    public void HideHover()
    {
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, counter / duration);

        }

        if (destroyAfterFade)
            Destroy(transform.parent.gameObject);
    }

    public void Fade()
    {
        DoFade(canvasGroup.alpha, faded ? 1 : 0);

        faded = !faded;
    }

    private void DoFade(float start, float end)
    {
        float counter = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            }
        
        if (destroyAfterFade)
            Destroy(transform.parent.gameObject);
    }

    public bool Faded()
    {
        return faded;
    }
}
