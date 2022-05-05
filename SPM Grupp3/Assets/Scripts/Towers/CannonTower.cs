using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Tower
{
    [Header("Amount To Upgrade")]
    [SerializeField] private float upgradeDamageAmount;
    [SerializeField] private float upgradeRangeAmount;
    [SerializeField] private float upgradeFireRateAmount;

    [Header("DubbelShot")]
    [SerializeField] private bool shootTwice = false;

    [Header("Upgrade Cost")]
    [SerializeField] private float level1Cost;
    [SerializeField] private float level2Cost;
    [SerializeField] private float level3Cost;

    [Header("Purchased Upgrades")]
    [SerializeField] private bool level1UpgradePurchased = false;
    [SerializeField] private bool level2UpgradePurchased = false;
    [SerializeField] private bool level3UpgradePurchased = false;

    private float fireCountdown = 0f;
    private List<CannonTower> cannonTowers;

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.RegisterListener<TowerHitEvent>(HitTarget);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
        radius.SetActive(false);
        towerScript = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        LockOnTarget();

        if (target != null)
        {
            if (CanYouShoot())
            {
                Shoot();
                if (shootTwice)
                {
                    Invoke(nameof(DubbelShot), 0.1f);
                }
            }

        }
    }

    void DubbelShot()
    {
        Shoot();
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
        enemyTarget.TakeDamage(ShotDamage);
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

    void CheckAllPlacedTowers()
    {
        foreach (GameObject gO in BuildManager.instance.towersPlaced)
        {
            if (gO.GetComponent<CannonTower>() != null)
            {
                cannonTowers.Add(gO.GetComponent<CannonTower>());         
            }
        }
    }

    public override void TowerLevel1()
    {
        if (gM.SpendResources(level1Cost,0f) && !level1UpgradePurchased)
        {
            CheckAllPlacedTowers();
            foreach (CannonTower cT in cannonTowers)
            {
                cT.fireRate += upgradeFireRateAmount;
                cT.level1UpgradePurchased = true;
                cT.cannonTowers.Clear();
            }
            fireRate += upgradeFireRateAmount;
            cannonTowers.Clear();
            level1UpgradePurchased = true;
        }      
    }
    public override void TowerLevel2()
    {
        if (gM.SpendResources(level2Cost, 0f) && !level2UpgradePurchased && level1UpgradePurchased)
        {
            CheckAllPlacedTowers();
            foreach (CannonTower cT in cannonTowers)
            {
                cT.ShotDamage = upgradeDamageAmount;
                cT.level2UpgradePurchased = true;
                cT.cannonTowers.Clear();
            }
            ShotDamage = upgradeDamageAmount;
            cannonTowers.Clear();
            level2UpgradePurchased = true;
        }
    }
    public override void TowerLevel3()
    {
        if (gM.SpendResources(level3Cost, 0f) && !level3UpgradePurchased && level2UpgradePurchased && level1UpgradePurchased)
        {
            CheckAllPlacedTowers();
            foreach (CannonTower cT in cannonTowers)
            {
                cT.shootTwice = true;
                cT.level3UpgradePurchased = true;
                cT.cannonTowers.Clear();
            }
            shootTwice = true;
            cannonTowers.Clear();
            level3UpgradePurchased = true;
        }
        
    }
}
