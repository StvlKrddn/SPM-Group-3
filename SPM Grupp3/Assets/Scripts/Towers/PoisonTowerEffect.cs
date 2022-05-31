using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTowerEffect : MonoBehaviour
{
    public Tower poisonTower;
    public void HitByPoison(float ticks, GameObject hitEffect, float dps, float currentHealthDamage, float range)
    {
        poisonTower = GetComponent<PoisonTower>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider enemyCollider in colliders)
        {
            EnemyController enemyController = enemyCollider.GetComponent<EnemyController>();
            if (enemyCollider.GetComponent<EnemyController>())
            {
                GameObject poisonEffect = Instantiate(hitEffect, enemyCollider.transform);
                Destroy(poisonEffect, ticks);
                if (enemyController.PoisonTickTimers.Count <= 0)
                {
                    enemyController.PoisonTickTimers.Add(ticks);
                    StartCoroutine(PoisonTick(dps, currentHealthDamage, enemyController));
                }
            }
        }
    }

    private IEnumerator PoisonTick(float dps, float maxHealthDamage, EnemyController enemyController)
    {
        while (enemyController.PoisonTickTimers.Count > 0)
        {
            for (int i = 0; i < enemyController.PoisonTickTimers.Count; i++)
            {
                enemyController.PoisonTickTimers[i]--;
            }
            enemyController.TakeDamage(dps);
            enemyController.TakeDamage(enemyController.Health * maxHealthDamage);
            enemyController.PoisonTickTimers.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
