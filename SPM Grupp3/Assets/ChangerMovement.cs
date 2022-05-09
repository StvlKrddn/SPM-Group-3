using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangerMovement : MonoBehaviour
{
    private bool mFaded = false;

    public float Duration = 0.4f;

    public float moveSpeed = 1;

    private void Start()
    {
        Fade();
    }

    public void Fade()
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, mFaded ? 1 : 0));

        mFaded = !mFaded;
    }

    public IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while(counter < Duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / Duration);
            yield return null;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this);
    }

    private void Update()
    {
        this.transform.Translate(Vector3.up * Time.deltaTime);
    }
}
