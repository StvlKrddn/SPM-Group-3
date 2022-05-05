using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTower : Tower
{
    [SerializeField] private float splashRadius = 1f;
    [SerializeField] private float splashDamage = 20f;
    [SerializeField] private float amountUpgradeSplashRadius;
    [SerializeField] private float amountUpgradeSplashDamage;
    public float fireCountdown = 0f;
    /*    [SerializeField] private GameObject shot;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject radius;*/

    private List<MissileTower> missileTowers = new List<MissileTower>();
    public float SplashRadius { get { return splashRadius; } set { splashRadius = value; } }
    public float SplashDamage { get { return splashDamage; } set { splashDamage = value; } }
    private float shotsFired = 0;
    private bool thirdShot = false;
    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.RegisterListener<TowerHitEvent>(HitTarget);
        towerScript = this;
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
            }
        }
    }

    public override void HitTarget(TowerHitEvent eventInfo)
    {
        if (target != null)
        {
            EnemyController enemyTarget = eventInfo.enemyHit.GetComponent<EnemyController>();
            GameObject effectInstance = Instantiate(eventInfo.hitEffect, enemyTarget.transform.position, enemyTarget.transform.rotation);

            Destroy(effectInstance, 1f);
            TypeOfShot(enemyTarget);
            /*Destroy(bullet.gameObject, 2f);*/
        }
    }

    /*    public override void HitTarget()
        {
            if (target != null)
            {
                EnemyController enemyTarget = target.GetComponent<EnemyController>();
                GameObject effectInstance = Instantiate(onHitEffect, bullet.gameObject.transform.position, bullet.gameObject.transform.rotation);

                Destroy(effectInstance, 2f);
                TypeOfShot(enemyTarget);
                Destroy(bullet.gameObject);
            }
        }*/

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

    public override void TypeOfShot(EnemyController enemyTarget)
    {
        print("Running? mis");
        if (thirdShot)
        {
            enemyTarget.HitBySplash(SplashRadius, SplashDamage * 2);
            enemyTarget.TakeDamage(shotDamage * 2);
        }
        enemyTarget.HitBySplash(SplashRadius, SplashDamage);
        enemyTarget.TakeDamage(shotDamage);
    }
    protected void Shoot()
    {
        shotsFired++;
        GameObject bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation);
        bulletGO.transform.parent = transform;
        bulletGO.SetActive(true);
        bullet = bulletGO.GetComponent<Shot>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
    void CheckAllPlacedTowers()
    {
        foreach (GameObject gO in BuildManager.instance.towersPlaced)
        {
            if (gO.GetComponent<MissileTower>() != null)
            {
                missileTowers.Add(gO.GetComponent<MissileTower>());
            }
        }
    }

    public override void TowerLevel1()
    {
        CheckAllPlacedTowers();
        foreach (MissileTower mT in missileTowers)
        {
            mT.splashRadius += amountUpgradeSplashRadius;
        }
        splashRadius += amountUpgradeSplashRadius;
        missileTowers.Clear();
    }
    public override void TowerLevel2()
    {
        CheckAllPlacedTowers();
        foreach (MissileTower mT in missileTowers)
        {
            mT.splashDamage += amountUpgradeSplashDamage;
        }
        splashDamage += amountUpgradeSplashDamage;
        missileTowers.Clear();
    }
    public override void TowerLevel3()
    {
        CheckAllPlacedTowers();
        foreach (MissileTower mT in missileTowers)
        {
            mT.thirdShot = true;
        }
        thirdShot = true;
        missileTowers.Clear();
    }
}
