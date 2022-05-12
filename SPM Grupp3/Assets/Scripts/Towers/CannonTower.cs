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





    // Start is called before the first frame update
    void Start()
    {

        /*        CheckLevels();*/
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
        if (eventInfo.towerGO == gameObject)
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

    public override void TowerLevel1()
    {
        base.TowerLevel1();

        if (tUC.GetUpgradesPurchased() == 0 && gM.SpendResources(level1Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            CannonTower cT = tUC.ClickedTower.GetComponent<CannonTower>();
            cT.fireRate += upgradeFireRateAmount;
        }      
    }

    public override void TowerLevel2()
    {
        base.TowerLevel2();

        if (tUC.GetUpgradesPurchased() == 1 && gM.SpendResources(level2Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            CannonTower cT = tUC.ClickedTower.GetComponent<CannonTower>();

            cT.ShotDamage = upgradeDamageAmount;

            GameObject towerUpgradeVisual1 = cT.gameObject.transform.GetChild(1).gameObject;
            GameObject towerUpgradeVisual2 = cT.gameObject.transform.GetChild(2).gameObject;

            towerUpgradeVisual1.SetActive(false);
            towerUpgradeVisual2.SetActive(true);
        }
    }

    public override void TowerLevel3()
    {
        base.TowerLevel3();

        if (tUC.GetUpgradesPurchased() == 2 && gM.SpendResources(level3Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            CannonTower cT = tUC.ClickedTower.GetComponent<CannonTower>();
            cT.shootTwice = true;
        }     
    }
}
