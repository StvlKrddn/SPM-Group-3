using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTowerEffect : MonoBehaviour
{
    private float effectDuration = 3f;
    private EnemyController enemyController;

	public void HitBySlow(EnemyController enemy, float slowProc, float radius, bool areaOfEffect, bool stun)
    {
        if (!areaOfEffect)
        {
            enemyController = enemy;
            SlowEnemy(slowProc);
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider enemyCollider in colliders)
            {
                if (enemyCollider.GetComponent<EnemyController>())
                {
                    enemyController = enemyCollider.GetComponent<EnemyController>();
                    if (stun)
                    {
                        slowProc = 0;
                    }
                    SlowEnemy(slowProc);
                }
            }
        }
    }

    private void SlowEnemy(float slowProc)
    {
        enemyController.Speed = enemyController.DefaultSpeed * slowProc;
        enemyController.HitBySlow();
        StartCoroutine(SlowDuration());
    }

    private IEnumerator SlowDuration()
    {
        yield return new WaitForSeconds(effectDuration);
        enemyController.Speed = enemyController.DefaultSpeed;
        enemyController.ResetAnimator();
    }
}
