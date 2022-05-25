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
    public bool thirdShot = false;

    [Header("Upgrade Cost")]
    [SerializeField] private float level1Cost;
    [SerializeField] private float level2Cost;
    [SerializeField] private float level3Cost;

    private float fireCountdown = 0f;
    private float shotsFired = 0;
    private bool shotAlready = false;

    public float costForUpgrade;
    public float ShotsFired { get { return shotsFired; } set { shotsFired = value; } }
    public float SplashRadius { get { return splashRadius; } set { splashRadius = value; } }
    public float SplashDamage { get { return splashDamage; } set { splashDamage = value; } }

    public Tower TowerScript 
    {
        get 
        {
            if (towerScript == null)
            {
                towerScript = this;
            }
            return towerScript;
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

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.RegisterListener<TowerHitEvent>(HitTarget);
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
        if (eventInfo.towerGO == gameObject)
        {
            if (target != null)
            {
                EnemyController enemyTarget = eventInfo.enemyHit.GetComponent<EnemyController>();
                GameObject effectInstance = Instantiate(eventInfo.hitEffect, enemyTarget.transform.position, enemyTarget.transform.rotation);

                Destroy(effectInstance, 1f);
                
                if (!shotAlready)
                {
                    bullet.DecideTypeOfShot("Missile");
                    shotAlready = true;
                }
                Invoke("Coldown", 0.3f);
            }
        }
    }
    void Coldown()
    {
        shotAlready = false;
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
    public override void ShowUpgradeUI(Transform towerMenu)
    {
        for (int i = 0; i < towerMenu.childCount; i++)
        {
            if (towerMenu.GetChild(i).gameObject.name.Equals("UpgradeMissilePanel"))
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
            MissileTower mT = tUC.ClickedTower.GetComponent<MissileTower>();
            Level1(mT.gameObject);
        }
    }
    protected override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gM.SpendResources(level2Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            MissileTower mT = tUC.ClickedTower.GetComponent<MissileTower>();
            Level2(mT.gameObject);
        }
        
    }
    protected override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gM.SpendResources(level3Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            MissileTower mT = tUC.ClickedTower.GetComponent<MissileTower>();
            Level3(mT.gameObject);
        }
    }

    protected override void Level1(GameObject tower)
    {
        tower.GetComponent<MissileTower>().splashRadius += amountUpgradeSplashRadius;
    }

    protected override void Level2(GameObject tower)
    {
        tower.GetComponent<MissileTower>().splashDamage += amountUpgradeSplashDamage;
    }

    protected override void Level3(GameObject tower)
    {   
        
        tower.GetComponent<MissileTower>().thirdShot = true;
    }
}
