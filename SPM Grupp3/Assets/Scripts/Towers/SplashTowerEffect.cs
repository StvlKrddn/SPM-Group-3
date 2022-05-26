using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTowerEffect : MonoBehaviour
{
    public virtual void HitBySplash(float radius, float splashDamage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider enemyCollider in colliders)
        {
            if (enemyCollider.GetComponent<EnemyController>())
            {
                enemyCollider.GetComponent<EnemyController>().TakeDamage(splashDamage);
            }
        }
    }
}
