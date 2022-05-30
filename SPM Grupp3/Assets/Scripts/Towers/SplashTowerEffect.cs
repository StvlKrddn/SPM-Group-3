using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTowerEffect : MonoBehaviour
{
    public void HitBySplash(float radius, float splashDamage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider enemyCollider in colliders)
        {
            EnemyController enemyController = enemyCollider.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(splashDamage);
            }
        }
    }
}
