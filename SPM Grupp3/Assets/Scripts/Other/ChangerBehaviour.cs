using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangerBehaviour : MonoBehaviour
{
    private bool mFaded = false;
    private Transform lookAt;

    public float Duration = 0.4f;

    public float moveSpeed = 1;

    private void Start()
    {
       
        lookAt = GameObject.FindGameObjectWithTag("Look").transform;
        Faded();
    }

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

        Destroy(transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate( Vector3.up * Time.deltaTime * moveSpeed);
    }

    private void LateUpdate()
    {
       transform.LookAt(lookAt);
       transform.Rotate(0, 180, 0);
    }
}
