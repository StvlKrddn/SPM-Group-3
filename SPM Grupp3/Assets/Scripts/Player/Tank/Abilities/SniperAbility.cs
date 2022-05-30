using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAbility : BulletBehavior
{
    [SerializeField] float damageOfAbility;

	protected override void OnBecameInvisible()
	{
		Destroy(gameObject, 0.01f);
	}

	protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController target = other.GetComponent<EnemyController>();
            EnemyController[] enemyControllers = FindObjectsOfType(typeof(EnemyController)) as EnemyController[];
            foreach (EnemyController enemy in enemyControllers)
            {
                if (target.GetType() == enemy.GetType())
                {
                    enemy.TakeDamage(damageOfAbility);
                }
            }
            Destroy(gameObject, 0.01f);
        }
    }
}
