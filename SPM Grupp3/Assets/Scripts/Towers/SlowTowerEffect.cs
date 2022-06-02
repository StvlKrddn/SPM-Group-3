using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTowerEffect : MonoBehaviour
{
    private float effectDuration = 9f;
    

	public void HitBySlow(EnemyController enemy, float slowProc, float radius, bool areaOfEffect, bool stun)
    {
        if (!areaOfEffect)
        {
            
            SlowEnemy(slowProc, enemy);
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider enemyCollider in colliders)
            {
                if (enemyCollider.GetComponent<EnemyController>())
                {   
                    EnemyController tempenemy = enemyCollider.GetComponent<EnemyController>();
                    if (stun)
                    {
                        slowProc = 0;
                    }
                    SlowEnemy(slowProc, tempenemy);
                }
            }
        }
    }

    private void SlowEnemy(float slowProc, EnemyController enemy)
    {
        enemy.Speed = enemy.DefaultSpeed * slowProc;
        enemy.HitBySlow();
        StartCoroutine(SlowDuration(enemy));
    }

    private IEnumerator SlowDuration(EnemyController enemy)
    {
        yield return new WaitForSeconds(effectDuration);
        enemy.Speed = enemy.DefaultSpeed;
        enemy.ResetAnimator();
    }
}
