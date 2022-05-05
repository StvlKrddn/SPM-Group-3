using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTower : Tower
{
    [SerializeField] private float splashRadius = 1f;
    [SerializeField] private float splashDamage = 20f;

    [Header("Amount To Upgrade")]
    [SerializeField] private float amountUpgradeSplashRadius;
    [SerializeField] private float amountUpgradeSplashDamage;

    [Header("ThirdShot Dubble Damage")]
    [SerializeField] private bool thirdShot = false;

    [Header("Upgrade Cost")]
    [SerializeField] private float level1Cost;
    [SerializeField] private float level2Cost;
    [SerializeField] private float level3Cost;

    [Header("Purchased Upgrades")]
    [SerializeField] private bool level1UpgradePurchased = false;
    [SerializeField] private bool level2UpgradePurchased = false;
    [SerializeField] private bool level3UpgradePurchased = false;

    private float fireCountdown = 0f;
    private float shotsFired = 0;
    private List<MissileTower> missileTowers = new List<MissileTower>();

    public float SplashRadius { get { return splashRadius; } set { splashRadius = value; } }
    public float SplashDamage { get { return splashDamage; } set { splashDamage = value; } }




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

    public override void TypeOfShot(EnemyController enemyTarget)
    {
        if (thirdShot && shotsFired % 3 == 0)
        {
            enemyTarget.HitBySplash(SplashRadius, SplashDamage * 2);
            enemyTarget.TakeDamage(ShotDamage * 2);
        }
        else
        {
            enemyTarget.HitBySplash(SplashRadius, SplashDamage);
            enemyTarget.TakeDamage(ShotDamage);
        }
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
        if (gM.SpendResources(level1Cost, 0f) && !level1UpgradePurchased)
        {
            CheckAllPlacedTowers();
            foreach (MissileTower mT in missileTowers)
            {
                mT.splashRadius += amountUpgradeSplashRadius;
                mT.level1UpgradePurchased = true;
                mT.missileTowers.Clear();
            }
            splashRadius += amountUpgradeSplashRadius;
            missileTowers.Clear();
            level1UpgradePurchased = true;
        }
    }
    public override void TowerLevel2()
    {
        if (gM.SpendResources(level2Cost, 0f) && !level2UpgradePurchased && level1UpgradePurchased)
        {
            CheckAllPlacedTowers();
            foreach (MissileTower mT in missileTowers)
            {
                mT.splashDamage += amountUpgradeSplashDamage;
                mT.level2UpgradePurchased = true;
                mT.missileTowers.Clear();
            }
            splashDamage += amountUpgradeSplashDamage;
            missileTowers.Clear();
            level2UpgradePurchased = true;
        }
        
    }
    public override void TowerLevel3()
    {
        if (gM.SpendResources(level3Cost, 0f) && !level3UpgradePurchased && level2UpgradePurchased && level1UpgradePurchased)
        {
            CheckAllPlacedTowers();
            foreach (MissileTower mT in missileTowers)
            {
                mT.thirdShot = true;
                mT.level3UpgradePurchased = true;
                mT.missileTowers.Clear();
            }
            thirdShot = true;
            missileTowers.Clear();
            level3UpgradePurchased = true;
        }
        
    }

    public override void CheckLevels()
    {

    }
}
