using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private float maxHealth = 50f;

    private float currentHealth;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void ModifyHealth(float amount)
    {
        currentHealth += amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            ModifyHealth(other.gameObject.GetComponent<EnemyBullet>().GetDamage());
        }
    }

    // testing the Healthbar degeneration
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ModifyHealth(-10);
        }
    }
}
