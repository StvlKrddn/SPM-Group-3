using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    [Header("Random components")]
    [SerializeField] private Transform garage;
    private TankController tankController;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void Awake()
    { 
        currentHealth = maxHealth;
        tankController = GetComponent<TankController>();
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
                //tankController.MoveToGarage();
            }

            Destroy(enemyBullet);
        }
    }    
    
    public void ModifyHealth(float amount)
    {
        currentHealth -= amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }
}
