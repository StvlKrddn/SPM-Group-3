using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTowerEffect : MonoBehaviour
{
    private EnemyController enemyController;

    public void HitBySlow(float slowProc, float radius, bool areaOfEffect, bool stun)
    {   

        if (!areaOfEffect)
        {           
            enemyController = gameObject.GetComponent<Shot>().target.GetComponent<EnemyController>(); 
            enemyController.speed *= slowProc;
            StartCoroutine(SlowDuration(enemyController));
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
                        enemyController.speed = 0;
                        StartCoroutine(SlowDuration(enemyController));
                        return;
                    }                   
                    enemyController.speed *= slowProc;
                    StartCoroutine(SlowDuration(enemyController));
                }
            }
        }
    }

    private IEnumerator SlowDuration(EnemyController enemyController)
    {
        yield return new WaitForSeconds(3f);
        enemyController.speed = enemyController.DefaultSpeed;        
    }
}