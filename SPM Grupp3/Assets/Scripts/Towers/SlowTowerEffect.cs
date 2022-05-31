using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTowerEffect : MonoBehaviour
{
	public void HitBySlow(EnemyController enemy, float slowProc, float radius, bool areaOfEffect, bool stun)
    {
        if (!areaOfEffect)
        {           
            enemy.Speed = slowProc;
            StartCoroutine(SlowDuration(enemy));
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider enemyCollider in colliders)
            {
                if (enemyCollider.GetComponent<EnemyController>())
                {
                    EnemyController enemyController = enemyCollider.GetComponent<EnemyController>();
                    if (stun)
                    {
                        enemyController.Speed = 0;
                        StartCoroutine(SlowDuration(enemyController));
                        return;
                    }
                    enemyController.Speed = slowProc;
                    StartCoroutine(SlowDuration(enemyController));
                }
            }
        }
    }

    private IEnumerator SlowDuration(EnemyController enemyController)
    {
        yield return new WaitForSeconds(3f);
        enemyController.Speed = enemyController.DefaultSpeed;
    }
}
