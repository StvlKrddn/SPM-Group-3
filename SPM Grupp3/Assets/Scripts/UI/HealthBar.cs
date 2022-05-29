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
    private float fillAmount;

    public float FillAmount { get { return fillAmount; } set { fillAmount = value; } }

    private void Awake()
    {
        lookAt = GameObject.FindGameObjectWithTag("Look").transform;
        slider = GetComponentInChildren<Slider>();
        //GetComponentInParent<Health>().UpdateHealthBar += HandleHealthChanged;
        //GetComponentInParent<Health>().UpdateHealthBar += HandleHealthChanged;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void HandleHealthChanged(float currentHealth)
    {
        if (currentHealth > 0)
        {
            StartCoroutine(UpdateHealthBar(currentHealth));
        }
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

        if (this.name.Equals("GameManager"))
            return;

        transform.LookAt(lookAt);

        if(this.name.Equals("TankUI"))
            transform.Rotate(90, 180, 0);
        else
            transform.Rotate(0, 180, 0);
    }
}
