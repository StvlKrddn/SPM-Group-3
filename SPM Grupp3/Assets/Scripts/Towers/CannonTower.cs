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

    [Header("Visual Upgrades")]
    public Mesh cannonLevel2;

    private float fireCountdown = 0f;
    private List<CannonTower> cannonTowers = new List<CannonTower>();

    private TowerUpgradeCotroller tUC;


    // Start is called before the first frame update
    void Start()
    {
        print("start");
        CheckLevels();
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

    public override void ShowUpgradeUI(GameObject medium, GameObject infoView)
    {
        if (infoView.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            infoView.transform.GetChild(0).gameObject.SetActive(false);
            medium.SetActive(true);
        }
        else
        {
            infoView.transform.GetChild(0).gameObject.SetActive(true);
            medium.SetActive(false);
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
        gM = FindObjectOfType<GameManager>();
        tUC = TowerUpgradeCotroller.instance;

        if ((gM.SpendResources(level1Cost,0f) && tUC.GetUpgradesPurchased(this) == 0) || tUC.GetUpgradesPurchased(this) > 2)
        {
            CheckAllPlacedTowers();
            tUC.IncreaseUpgradesPurchased(this);
            foreach (CannonTower cT in cannonTowers)
            {
                FireRate(cT);         
            }

            cannonTowers.Clear();
        }      
    }

    void FireRate(CannonTower cT)
    {
        cT.fireRate += upgradeFireRateAmount;
    }

    public override void TowerLevel2()
    {
        gM = FindObjectOfType<GameManager>();
        tUC = TowerUpgradeCotroller.instance;

        if (gM.SpendResources(level2Cost, 0f) && tUC.GetUpgradesPurchased(this) == 1)
        {

            CheckAllPlacedTowers();
            tUC.IncreaseUpgradesPurchased(this);
            foreach (CannonTower cT in cannonTowers)
            {

                damageAndVisualUpgrade(cT);
            }

            cannonTowers.Clear();
        }
    }

    void damageAndVisualUpgrade(CannonTower cT)
    {
        cT.ShotDamage = upgradeDamageAmount;

        GameObject towerUpgradeVisual1 = cT.gameObject.transform.GetChild(1).gameObject;
        GameObject towerUpgradeVisual2 = cT.gameObject.transform.GetChild(2).gameObject;

        towerUpgradeVisual1.SetActive(false);
        towerUpgradeVisual2.SetActive(true);
    }

    public override void TowerLevel3()
    {
        gM = FindObjectOfType<GameManager>();
        tUC = TowerUpgradeCotroller.instance;

        if (gM.SpendResources(level3Cost, 0f) && tUC.GetUpgradesPurchased(this) == 2)
        {
            tUC.IncreaseUpgradesPurchased(this);
            CheckAllPlacedTowers();
            foreach (CannonTower cT in cannonTowers)
            {
                DubbleShotUpgrade(cT);
            }
            cannonTowers.Clear();
        }     
    }

    void DubbleShotUpgrade(CannonTower cT)
    {
        cT.shootTwice = true;
    }

    public override void CheckLevels()
    {
        tUC = TowerUpgradeCotroller.instance;
        print(tUC.GetUpgradesPurchased(this));

        if (tUC.GetUpgradesPurchased(this) > 0)
        {
            print("running FR");
            FireRate(this);
            if (tUC.GetUpgradesPurchased(this) > 1)
            {
                print("running DMG Vis");
                damageAndVisualUpgrade(this);
                if (tUC.GetUpgradesPurchased(this) > 2)
                {
                    print("running DS");
                    DubbleShotUpgrade(this);
                }
            }
        }
        
        
    }
}
