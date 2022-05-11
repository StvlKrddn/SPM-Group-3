using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAbility : BulletBehavior
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("test");
            EnemyController target = other.GetComponent<EnemyController>();
            EnemyController[] enemyControllers = FindObjectsOfType(typeof(EnemyController)) as EnemyController[];
            foreach (EnemyController enemy in enemyControllers)
            {
                if (target.GetType() == enemy.GetType())
                {
                    enemy.TakeDamage(damage);
                }
            }
            Destroy(gameObject, 0.01f);
        }
    }
}
