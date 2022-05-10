using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    private TankState player;
    private EnemyController enemy;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void Awake()
    {
        if (gameObject.GetComponent<TankState>())
        {
            player = GetComponent<TankState>();
            currentHealth = player.Health;
        }
        else if (gameObject.GetComponent<EnemyController>())
        {
            enemy = GetComponent<EnemyController>();
            currentHealth = enemy.Health;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            
            EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();

            ModifyHealth(enemyBullet.damage);
            player.TakeDamage(enemyBullet.damage);

            /*if(currentHealth <= 0)
            {
                currentHealth = maxHealth;
                ModifyHealth(-1.0f);
            }*/
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            if (gameObject.CompareTag("Tank"))
            {
                return;
            }
            GameObject towerBullet = other.gameObject;
            Tower tower = towerBullet.GetComponent<Shot>().getTowerShotCameFrom();

            ModifyHealth(tower.ShotDamage);

            if (currentHealth <= 0)
            {
                currentHealth = maxHealth;
                ModifyHealth(-1.0f);
            }
        }
        if (other.gameObject.CompareTag("PlayerShots"))
        {
            if (gameObject.CompareTag("Tank"))
            {
                return;
            }
            BulletBehavior playerBullet = other.GetComponent<BulletBehavior>();

            ModifyHealth(playerBullet.BulletDamage);
            enemy.TakeDamage(playerBullet.BulletDamage);

            /*if (currentHealth <= 0)
            {
                currentHealth = maxHealth;
                ModifyHealth(-1.0f);
            }*/
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TankState player = gameObject.GetComponent<TankState>();
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            ModifyHealth(enemy.MeleeDamage);

            player.TakeDamage(enemy.MeleeDamage);

            /*if (currentHealth <= 0)
            {
                currentHealth = maxHealth;
                ModifyHealth(-1.0f);
            }*/
        }

    }

    public void ModifyHealth(float amount)
    {
        currentHealth -= amount;

        float currentHealthPct = currentHealth / maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthPctChanged(1);
    }
}
