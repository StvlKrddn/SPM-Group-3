using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    private float maxHealth;
    private float currentHealth;

    private TankState player;
    private EnemyController enemy;

    public event Action<float> UpdateHealthBar = delegate { };
    private HealthBar healthBar;

    private void Awake()
    {
      //  healthBar = GetComponentInChildren<HealthBar>();
   
        if (gameObject.GetComponent<TankState>())
        {
            player = GetComponent<TankState>();
            maxHealth = player.Health;
      //      healthBar.slider.value = maxHealth;
        }
        else if (gameObject.GetComponent<EnemyController>())
        {
            enemy = GetComponent<EnemyController>();
            maxHealth = enemy.Health;
         //   healthBar.slider.value = maxHealth;
        }
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            
            EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();

       //     ModifyHealth(enemyBullet.damage);
            player.TakeDamage(enemyBullet.damage);
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            if (gameObject.CompareTag("Tank"))
            {
                return;
            }
            GameObject towerBullet = other.gameObject;
            Tower tower = towerBullet.GetComponent<Shot>().getTowerShotCameFrom();

        //    ModifyHealth(tower.ShotDamage);
        }
        if (other.gameObject.CompareTag("PlayerShots"))
        {
            if (gameObject.CompareTag("Tank"))
            {
                return;
            }
            BulletBehavior playerBullet = other.GetComponent<BulletBehavior>();

         //   ModifyHealth(playerBullet.BulletDamage);
            enemy.TakeDamage(playerBullet.BulletDamage);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TankState player = gameObject.GetComponent<TankState>();
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

      //      ModifyHealth(enemy.MeleeDamage);

            player.TakeDamage(enemy.MeleeDamage);
        }

    }

    public void ModifyHealth(float amount)
    {
        currentHealth -= amount;

        /*        float currentHealthPct = currentHealth / maxHealth;*/
     //   UpdateHealthBar(currentHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(maxHealth);
    }
}
