using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Transform target;
    public float shotSpeed = 1f;
    [SerializeField] private float shotDamage = 5000f;
    public GameObject hitEffect;
    [SerializeField] private int poisonTicks = 5;
    [SerializeField] private int poisonDamagePerTick = 25;

    [SerializeField] private float slowProc = 0.7f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = shotSpeed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        EnemyController enemyTarget = target.GetComponent<EnemyController>();
        GameObject effectInstance = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 1f);

<<<<<<< HEAD
        TypeOfShot(enemyTarget);
        
/*        Destroy(target.gameObject);*/
        Destroy(gameObject);
=======
        enemyTarget.TakeDamage(shotDamage);
    //    Destroy(target.gameObject);
          Destroy(gameObject);
>>>>>>> main
    }

    void TypeOfShot(EnemyController enemyTarget)
    {
        switch (gameObject.tag)
        {
            case "PoisonTower":
                shotDamage = 0f;
                enemyTarget.HitByPoison(poisonTicks, poisonDamagePerTick);
                break;
            case "SlowTower":
                shotDamage = 0f;
                enemyTarget.HitBySlow(slowProc);
                break;
            case "MissileTower":
                break;
            default:
                enemyTarget.TakeDamage(shotDamage);
                break;
        }
            
    }
}
