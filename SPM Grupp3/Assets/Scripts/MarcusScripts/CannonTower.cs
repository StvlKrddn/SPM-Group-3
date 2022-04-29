using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Tower
{

    // Start is called before the first frame update
    void Start()
    {
        radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
        radius.SetActive(false);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        LockOnTarget();

        if (CanYouShoot())
        {
            Shoot();
            EnemyController enemyTarget = target.gameObject.GetComponent<EnemyController>();
            
            if (bullet.CheckIfProjectileHit())
            {
                TypeOfShot(enemyTarget);
            }
        }

    }
    protected override void TypeOfShot(EnemyController enemyTarget)
    {
        enemyTarget.TakeDamage(shotDamage);
    }
    protected void Shoot()
    {
        GameObject bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation);
        bulletGO.transform.parent = transform;
        bulletGO.SetActive(true);
        bullet = bulletGO.GetComponent<Shot>();



        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
