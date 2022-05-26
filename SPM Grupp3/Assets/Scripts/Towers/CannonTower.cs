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

    public float costForUpgrade;

    private float fireCountdown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
    }

    private void OnEnable()
    {
        EventHandler.RegisterListener<TowerHitEvent>(HitTarget);
    }

    void OnDestroy()
    {
        EventHandler.UnregisterListener<TowerHitEvent>(HitTarget);
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

    public override float UpgradeCostUpdate()
    {
        base.TowerLevel1();
        switch (tUC.GetUpgradesPurchased())
        {
            case 0:
                costForUpgrade = level1Cost;
                break;
            case 1:
                costForUpgrade = level2Cost;
                break;
            case 2:
                costForUpgrade = level3Cost;
                break;
        }
        return costForUpgrade;
    }

    void DubbelShot()
    {
        Shoot();
    }

    public override void HitTarget(TowerHitEvent eventInfo)
    {
        if (eventInfo.towerGO == gameObject)
        {
            if (target != null)
            {
                EnemyController enemyTarget = eventInfo.enemyHit.GetComponent<EnemyController>();
                GameObject effectInstance = Instantiate(eventInfo.hitEffect, enemyTarget.transform.position, enemyTarget.transform.rotation);

                Destroy(effectInstance, 1f);
                bullet.DecideTypeOfShot("Cannon");
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

    protected void Shoot()
    {
        GameObject bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation, transform);
        bulletGO.SetActive(true);
        bullet = bulletGO.GetComponent<Shot>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public override void ShowUpgradeUI(Transform towerMenu)
    {
        for (int i = 0; i < towerMenu.childCount; i++)
        {
            if (towerMenu.GetChild(i).gameObject.name.Equals("UpgradeCannonPanel"))
            {
                GameObject menuToShow = towerMenu.GetChild(i).gameObject;
                menuToShow.transform.position = transform.position;
                menuToShow.SetActive(true);
            }
        }
    }

    protected override void TowerLevel1()
    {
        base.TowerLevel1();

        if (gM.SpendResources(level1Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            CannonTower cT = tUC.ClickedTower.GetComponent<CannonTower>();
            Level1(cT.gameObject);
        }      
    }

    protected override void Level1(GameObject tower)
    {
        tower.GetComponent<CannonTower>().fireRate += upgradeFireRateAmount;
    }

    protected override void TowerLevel2()
    {
        base.TowerLevel2();

        if (gM.SpendResources(level2Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            CannonTower cT = tUC.ClickedTower.GetComponent<CannonTower>();
            Level2(cT.gameObject);
        }
    }

    protected override void Level2(GameObject tower)
    {
        CannonTower cT = tower.GetComponent<CannonTower>();

        cT.ShotDamage = upgradeDamageAmount;

        GameObject towerUpgradeVisual1 = cT.transform.Find("Level1").gameObject;
        GameObject towerUpgradeVisual2 = cT.transform.Find("Level2").gameObject;

        towerUpgradeVisual1.SetActive(false);
        towerUpgradeVisual2.SetActive(true);
    }

    

    protected override void TowerLevel3()
    {
        base.TowerLevel3();

        if (gM.SpendResources(level3Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            CannonTower cT = tUC.ClickedTower.GetComponent<CannonTower>();
            Level3(cT.gameObject);
        }     
    }

    protected override void Level3(GameObject tower)
    {
        CannonTower cT = tower.GetComponent<CannonTower>();
        cT.shootTwice = true;
    }
}
