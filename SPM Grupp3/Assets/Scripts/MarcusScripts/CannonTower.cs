using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Tower
{
    private float fireCountdown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
        radius.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        LockOnTarget();

        if (CanYouShoot())
        {
            Shoot();
            
            if (bullet.CheckIfProjectileHit())
            {
                HitTarget();
            }
        }

    }
    private bool CanYouShoot()
    {
        if (fireCountdown <= 0f)
        {
            fireCountdown = 1f / fireRate;
            return true;
        }
        fireCountdown -= Time.deltaTime;
        return false;
    }
    public override void HitTarget()
    {
        EnemyController enemyTarget = target.GetComponent<EnemyController>();
        GameObject effectInstance = Instantiate(onHitEffect, transform.position, transform.rotation);
        
        Destroy(effectInstance, 1f);
        TypeOfShot(enemyTarget);
        Destroy(gameObject);
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
