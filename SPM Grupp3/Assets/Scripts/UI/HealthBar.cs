using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private Transform lookAt;
    [SerializeField] private float updateSpeedSeconds = 0.5f;
    //[SerializeField] private GameObject FollowPlayer;
    [SerializeField] private bool faceCamera = true;
    private float fillAmount;

    public float FillAmount { get { return fillAmount; } set { fillAmount = value; } }

    private void Awake()
    {
        lookAt = GameObject.FindGameObjectWithTag("Look").transform;
        slider = GetComponentInChildren<Slider>();
        //GetComponentInParent<Health>().UpdateHealthBar += HandleHealthChanged;
        //GetComponentInParent<Health>().UpdateHealthBar += HandleHealthChanged;
    }

    public void HandleHealthChanged(float currentHealth)
    {
        StartCoroutine(UpdateHealthBar(currentHealth));
        //UpdateHealthBar(currentHealth);
    }

    private IEnumerator UpdateHealthBar(float currentHealth)
    {
        float preChangePct = slider.value;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(preChangePct, currentHealth, elapsed / updateSpeedSeconds);
            yield return null;
        }

        slider.value = currentHealth;
    }

/*    public void UpdateHealthBar(float currentHealth)
    {
        print(currentHealth);
        slider.value = currentHealth;
    }*/

    private void LateUpdate()
    {
        if (faceCamera)
        {
            transform.LookAt(lookAt);
            transform.Rotate(0, 180, 0);
        }
    }
}
