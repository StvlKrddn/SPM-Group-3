using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Enemy : MonoBehaviour
{
    public Slider slider;
    private Transform lookAt;
    [SerializeField] private Animator healthAnimator;

    [SerializeField] private float updateSpeedSeconds = 0.5f;
    //[SerializeField] private GameObject FollowPlayer;
    private float fillAmount;

    public float FillAmount { get { return fillAmount; } set { fillAmount = value; } }

    private void Awake()
    {
        lookAt = GameObject.FindGameObjectWithTag("Look").transform;
        slider = GetComponentInChildren<Slider>();
        healthAnimator = gameObject.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void HandleHealthChanged(float currentHealth)
    {
        if (currentHealth > 0 && gameObject.activeSelf == true)
        {
            StartCoroutine(UpdateHealthBar(currentHealth));
        }
    }

    private IEnumerator UpdateHealthBar(float currentHealth)
    {
        float preChangePct = slider.value;
        float elapsed = 0f;

        if (currentHealth < slider.value)
            healthAnimator.Play("Hit");

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(preChangePct, currentHealth, elapsed / updateSpeedSeconds);
            yield return null;
        }

        slider.value = currentHealth;
    }

    public void UpdateHealthBarInstant(float currentHealth)
    {
        slider.value = currentHealth;
    }

    private void LateUpdate()
    {
        transform.LookAt(lookAt);

        transform.Rotate(0, 180, 0);

        if( slider.maxValue * 0.4f >= slider.value && slider.maxValue * 0.2f <= slider.value)
            healthAnimator.SetBool("Below40%", true);

        else if (slider.maxValue * 0.2f > slider.value)
            healthAnimator.SetBool("Below10%", true);
    }
}
