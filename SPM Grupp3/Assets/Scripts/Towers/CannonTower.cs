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

    [Space]
    [SerializeField] private Animator animator;

    private float fireCountdown = 0f;

    [System.NonSerialized] public float CostForUpgrade;
    public float FireRate { get { return fireRate; } set { fireRate = value; } }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        Radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        LockOnTarget();

        if (target != null)
        {
            if (CanYouShoot())
            {
               // animator.SetTrigger("Shooting");
                Shoot();
                if (shootTwice)
                {
                    Invoke(nameof(DubbelShot), 0.1f);
                }
            }
/*            else
            {
                animator.SetBool("Shooting", false);
            }*/

        }
    }

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

    void DubbelShot()
    {
        Shoot();
    }

    public override void HitTarget(GameObject target, GameObject hitEffect)
    {
        if (target != null && target.GetComponent<EnemyController>())
        {
            EnemyController enemyTarget = target.GetComponent<EnemyController>();
            Destroy(Instantiate(hitEffect, enemyTarget.transform.position + Vector3.up * 2, Quaternion.LookRotation(transform.position - enemyTarget.transform.position)), 1f);
            bullet.DecideTypeOfShot("Cannon");
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
        EventHandler.InvokeEvent(new PlaySoundEvent("Tower shot", towerShotSound));
        int bulletIndex = FindShot();
        if (bulletIndex < 0)
        {
            GameObject bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation, transform);
            bulletGO.SetActive(true);
            bullet = bulletGO.GetComponent<Shot>();
            shots.Add(bulletGO);
        }
        else
        {
            bullet = shots[bulletIndex].GetComponent<Shot>();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.gameObject.SetActive(true);
        }

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

        if (gameManager.SpendResources(level1Cost, 0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            CannonTower cannonTower = towerManager.ClickedTower.GetComponent<CannonTower>();
            Level1(cannonTower.gameObject);
        }      
    }

    protected override void Level1(GameObject tower)
    {
        CannonTower cannonTower = tower.GetComponent<CannonTower>();
        cannonTower.fireRate += upgradeFireRateAmount;
        GetVisualUpgrade(cannonTower);
    }

    protected override void TowerLevel2()
    {
        base.TowerLevel2();

        if (gameManager.SpendResources(level2Cost, 0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            CannonTower cannonTower = towerManager.ClickedTower.GetComponent<CannonTower>();
            Level2(cannonTower.gameObject);
        }
    }

    protected override void Level2(GameObject tower)
    {
        CannonTower cannonTower = tower.GetComponent<CannonTower>();

        cannonTower.ShotDamage += upgradeDamageAmount;

        

        level1Visual.SetActive(false);
        level2Visual.SetActive(true);
    }

    

    protected override void TowerLevel3()
    {
        base.TowerLevel3();

        animator = level3Visual.GetComponent<Animator>();

        if (gameManager.SpendResources(level3Cost, Level3MaterialCost))
        {
            towerManager.IncreaseUpgradesPurchased();
            CannonTower cannonTower = towerManager.ClickedTower.GetComponent<CannonTower>();
            Level3(cannonTower.gameObject);
        }     
    }

    protected override void Level3(GameObject tower)
    {
        
        CannonTower cannonTower = tower.GetComponent<CannonTower>();
        cannonTower.shootTwice = true;

        level2Visual.SetActive(false);
        level3Visual.SetActive(true);
    }
}
