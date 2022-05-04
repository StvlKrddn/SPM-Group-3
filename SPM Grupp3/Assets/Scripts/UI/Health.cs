using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void Awake()
    { 
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            GameObject enemyBullet = other.gameObject;

            ModifyHealth(enemyBullet.GetComponent<EnemyBullet>().damage);

            if(currentHealth <= 0)
            {
                currentHealth = maxHealth;
                ModifyHealth(-1.0f);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //GameObject enemy = other.gameObject;

            ModifyHealth(10);

            if (currentHealth <= 0)
            {
                currentHealth = maxHealth;
                ModifyHealth(-1.0f);
            }
        }
    }

    public void ModifyHealth(float amount)
    {
        currentHealth -= amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }
}
