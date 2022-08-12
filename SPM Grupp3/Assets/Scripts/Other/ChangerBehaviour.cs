using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangerBehaviour : MonoBehaviour
{
    

    private CanvasGroup canvasGroup;
    private Transform lookAt;

    [SerializeField] private float duration = 0.4f;

    [SerializeField] private float moveSpeed = 1;

    [SerializeField] private bool destroyAfterFade = true;
    [SerializeField] private bool faceCamera = true;
    [SerializeField] private bool stationary = false;

    private bool faded;

    private void OnEnable()
    {
        //transform.position = transform.parent.position;
        canvasGroup = GetComponent<CanvasGroup>();
        lookAt = GameObject.FindGameObjectWithTag("Look").transform;

        if (canvasGroup.alpha < 1)
            faded = true;
        else
            faded = false;

        Fade();
        Appear();
    }

    private void Fade()
    {
        StartCoroutine(DoFade(canvasGroup.alpha, faded ? 1 : 0));

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
            Destroy(gameObject);
    }

    private void Appear()
    {
        StartCoroutine(DoAppear());
    }

    private IEnumerator DoAppear()
    {
        float time = 0;

        while (time <= 0.5f)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.one * Mathfx.Berp(0f, 1f, time);

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stationary)
            transform.Translate( Vector3.up * Time.deltaTime * moveSpeed);
    }

    private void LateUpdate()
    {
        if (!faceCamera)
            return;

       transform.LookAt(lookAt);
       transform.Rotate(0, 180, 0);
    }
}
