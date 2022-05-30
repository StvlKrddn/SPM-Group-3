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

    public override float UpgradeCostUpdate()
    {
        base.TowerLevel1();
        switch (towerUpgradeController.GetUpgradesPurchased())
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
        radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
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
    public override void HitTarget(GameObject hit, GameObject hitEffect)
    {
        if (hit != null && hit.GetComponent<EnemyController>())
        {
            EnemyController enemyTarget = hit.GetComponent<EnemyController>();
            GameObject effectInstance = Instantiate(hitEffect, enemyTarget.transform.position, enemyTarget.transform.rotation);

            Destroy(effectInstance, 1f);
            
            if (!shotAlready)
            {
                bullet.DecideTypeOfShot("Missile");
                shotAlready = true;
            }
            Invoke("Cooldown", 0.3f);
        }
    }
    void Cooldown()
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
        int missileIndex = FindShot();
        GameObject bulletGO;
        if (missileIndex < 0)
        {
            bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation, transform);
            shots.Add(bulletGO);
        }
        else
        {
            bulletGO = shots[missileIndex];
            bulletGO.transform.position = firePoint.position;
            bulletGO.transform.rotation = firePoint.rotation;
        }
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
            towerUpgradeController.IncreaseUpgradesPurchased();
            MissileTower mT = towerUpgradeController.ClickedTower.GetComponent<MissileTower>();
            Level1(mT.gameObject);
        }
    }
    protected override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gM.SpendResources(level2Cost, 0f))
        {
            towerUpgradeController.IncreaseUpgradesPurchased();
            MissileTower mT = towerUpgradeController.ClickedTower.GetComponent<MissileTower>();
            Level2(mT.gameObject);
        }
        
    }
    protected override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gM.SpendResources(level3Cost, 0f))
        {
            towerUpgradeController.IncreaseUpgradesPurchased();
            MissileTower mT = towerUpgradeController.ClickedTower.GetComponent<MissileTower>();
            Level3(mT.gameObject);
        }
    }

    protected override void Level1(GameObject tower)
    {
        tower.GetComponent<MissileTower>().splashRadius += amountUpgradeSplashRadius;
    }

    protected override void Level2(GameObject tower)
    {
        MissileTower mT = tower.GetComponent<MissileTower>();

        GameObject towerUpgradeVisual1 = mT.transform.Find("Container").Find("Level1").gameObject;
        GameObject towerUpgradeVisual2 = mT.transform.Find("Container").Find("Level2").gameObject;

        towerUpgradeVisual1.SetActive(false);
        towerUpgradeVisual2.SetActive(true);

        tower.GetComponent<MissileTower>().splashDamage += amountUpgradeSplashDamage;
    }

    protected override void Level3(GameObject tower)
    {   
        
        tower.GetComponent<MissileTower>().thirdShot = true;
    }
}
