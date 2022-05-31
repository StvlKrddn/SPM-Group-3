using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTowerEffect : MonoBehaviour
{
    private float effectDuration = 3f;

	public void HitBySlow(EnemyController enemy, float slowProc, float radius, bool areaOfEffect, bool stun)
    {
        if (!areaOfEffect)
        {
            SlowEnemy(enemy, slowProc);
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
                        slowProc = 0;
                    }
                    SlowEnemy(enemyController, slowProc);
                }
            }
        }
    }

    private void SlowEnemy(EnemyController enemyController, float slowProc)
    {
        enemyController.Speed *= slowProc;
        enemyController.HitBySlow(slowProc);
        StartCoroutine(SlowDuration(enemyController));
    }

    private IEnumerator SlowDuration(EnemyController enemyController)
    {
        yield return new WaitForSeconds(effectDuration);
        enemyController.Speed = enemyController.DefaultSpeed;
        enemyController.ResetAnimator();
    }
}
