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

    private float fireCountdown = 0f;
    private float shotsFired = 0;


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
        if (eventInfo.towerGO == gameObject)
        {
            if (target != null)
            {
                EnemyController enemyTarget = eventInfo.enemyHit.GetComponent<EnemyController>();
                GameObject effectInstance = Instantiate(eventInfo.hitEffect, enemyTarget.transform.position, enemyTarget.transform.rotation);

                Destroy(effectInstance, 1f);
                TypeOfShot(enemyTarget);
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
    public override void ShowUpgradeUI(GameObject upgradeMenu)
    {
        upgradeMenu.SetActive(true);
        for (int i = 0; i < upgradeMenu.transform.childCount; i++)
        {
            if (upgradeMenu.transform.GetChild(i).gameObject.name.Equals("UpgradeMissilePanel"))
            {
                upgradeMenu.transform.GetChild(i).gameObject.SetActive(true);
                upgradeMenu.SetActive(true);
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
            mT.splashRadius += amountUpgradeSplashRadius;
        }
    }
    protected override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gM.SpendResources(level2Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            MissileTower mT = tUC.ClickedTower.GetComponent<MissileTower>();
            mT.splashDamage += amountUpgradeSplashDamage;
        }
        
    }
    protected override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gM.SpendResources(level3Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            MissileTower mT = tUC.ClickedTower.GetComponent<MissileTower>();
            mT.thirdShot = true;
        }
    }
}
