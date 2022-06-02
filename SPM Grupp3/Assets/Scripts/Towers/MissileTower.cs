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
    public bool ThirdShot = false;

    [Header("Upgrade Cost")]
    [SerializeField] private float level1Cost;
    [SerializeField] private float level2Cost;
    [SerializeField] private float level3Cost;

    [Space]
    [SerializeField] private Animator animator;

    private float fireCountdown = 0f;
    private float shotsFired = 0;
    private bool shotAlready = false;

    [System.NonSerialized] public float CostForUpgrade;
    public float ShotsFired { get { return shotsFired; } set { shotsFired = value; } }
    public float SplashRadius { get { return splashRadius; } set { splashRadius = value; } }
    public float SplashDamage { get { return splashDamage; } set { splashDamage = value; } }

    public override float UpgradeCostUpdate()
    {
        base.TowerLevel1();
        switch (towerManager.GetUpgradesPurchased())
        {
            case 0:
                CostForUpgrade = level1Cost;
                break;
            case 1:
                CostForUpgrade = level2Cost;
                break;
            case 2:
                CostForUpgrade = level3Cost;
                break;
        }
        return CostForUpgrade;
    }

    // Start is called before the first frame update
    void Start()
    {
        Radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {       
        LockOnTarget();

        if (target != null)
        {
            animator.SetTrigger("Shooting");
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
        EventHandler.InvokeEvent(new PlaySoundEvent("Tower shot", towerShotSound));
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

        if (bullet != null && target != null)
        {
            bullet.Seek(target);
        }
        else if (target == null)
        {
            bullet.gameObject.SetActive(false);
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
        if (gameManager.SpendResources(level1Cost, 0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            MissileTower missileTower = towerManager.ClickedTower.GetComponent<MissileTower>();
            Level1(missileTower.gameObject);
        }
    }
    protected override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gameManager.SpendResources(level2Cost, 0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            MissileTower missileTower = towerManager.ClickedTower.GetComponent<MissileTower>();
            Level2(missileTower.gameObject);
        }
        
    }
    protected override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gameManager.SpendResources(level3Cost, Level3MaterialCost))
        {
            towerManager.IncreaseUpgradesPurchased();
            MissileTower missileTower = towerManager.ClickedTower.GetComponent<MissileTower>();
            Level3(missileTower.gameObject);
        }
    }

    protected override void Level1(GameObject tower)
    {
        MissileTower missileTower = tower.GetComponent<MissileTower>();
        missileTower.splashRadius += amountUpgradeSplashRadius;

        GetVisualUpgrade(missileTower);
    }

    protected override void Level2(GameObject tower)
    {
        MissileTower missileTower = tower.GetComponent<MissileTower>();



        level1Visual.SetActive(false);
        level2Visual.SetActive(true);

        tower.GetComponent<MissileTower>().splashDamage += amountUpgradeSplashDamage;
    }

    protected override void Level3(GameObject tower)
    {   
        
        tower.GetComponent<MissileTower>().ThirdShot = true;
    }
}
