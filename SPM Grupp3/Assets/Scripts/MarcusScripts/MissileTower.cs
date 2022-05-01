using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTower : Tower
{
    [SerializeField] private float splashRadius = 1f;
    [SerializeField] private float splashDamage = 20f;
    public float fireCountdown = 0f;
    /*    [SerializeField] private GameObject shot;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject radius;*/
    public float SplashRadius { get { return splashRadius; } set { splashRadius = value; } }
    public float SplashDamage { get { return splashDamage; } set { splashDamage = value; } }
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

        if (target != null)
        {
            if (CanYouShoot())
            {
                Shoot();

                if (bullet.CheckIfProjectileHit())
                {
                    HitTarget();
                }
            }
        }
    }

    public override void HitTarget()
    {
        if (target != null)
        {
            EnemyController enemyTarget = target.GetComponent<EnemyController>();
            GameObject effectInstance = Instantiate(onHitEffect, bullet.gameObject.transform.position, bullet.gameObject.transform.rotation);

            Destroy(effectInstance, 2f);
            TypeOfShot(enemyTarget);
            Destroy(bullet.gameObject);
        }
    }

    private bool CanYouShoot()
    {
        if (fireCountdown <= 0f)
        {
            print("True");
            fireCountdown = 1f / fireRate;
            return true;
        }
        fireCountdown -= Time.deltaTime;
        return false;
    }

    protected override void TypeOfShot(EnemyController enemyTarget)
    {
        enemyTarget.HitBySplash(SplashRadius, SplashDamage);
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
