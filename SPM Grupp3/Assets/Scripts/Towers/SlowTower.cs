using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [SerializeField] private float slowProc = 0.7f;
    [SerializeField] private float slowRadius = 1f;
    [SerializeField] private bool areaOfEffect = false;
    public bool stunActive = false;
    [SerializeField] private float shotsBeforeStun;

    [Header("Amount To Upgrade")]
    [SerializeField] private float upgradeAmountSlowProc;
    [SerializeField] private float upgradeAmountSlowRadius;
    [SerializeField] private float stunDuration;
    [SerializeField] private float stunTimer;

    [Header("Upgrade Cost")]
    [SerializeField] private float level1Cost;
    [SerializeField] private float level2Cost;
    [SerializeField] private float level3Cost;

    private float currentShots = 0;

    public float costForUpgrade;

    private float fireCountdown = 0f;
    private float timer;

    public float SlowProc { get { return slowProc; } set { slowProc = value; } }
    public float ShotsBeforeStun { get { return shotsBeforeStun; } set { shotsBeforeStun = value; } }
    public float CurrentShots { get { return currentShots; } set { currentShots = value; } }
    public bool AreaOfEffect { get { return areaOfEffect; } set { areaOfEffect = value; } }


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
        radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {   if(!areaOfEffect)
        {
            LockOnTarget();
        }
       

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
        if (target != null)
        {
            bullet.DecideTypeOfShot("Slow");
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


    private void Shoot()
    {   
        int shotIndex = FindShot();
        if (!areaOfEffect)
        {
            GameObject bulletGO;
            if (shotIndex < 0)
            {
                bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation);
            }
            else
            {
                bulletGO = shots[shotIndex];
            }
            bulletGO.transform.parent = transform;
            bulletGO.SetActive(true);
            bullet = bulletGO.GetComponent<Shot>();

            if (bullet != null)
            {
                bullet.Seek(target);
            }
        }
        else
        {
            AOESlow(shotIndex);
        }
    }

    private void  AOESlow(int shotIndex)
    {
        if (currentShots <= 0 && stunActive)
        {
            currentShots = shotsBeforeStun;
        }
        else
        {
            currentShots -= 1;
        }

        GameObject effectInstance;
        if (shotIndex < 0)
        {
            effectInstance = Instantiate(onHitEffect, transform.position, transform.rotation);
        }
        else
        {
            effectInstance = shots[shotIndex];
        }

        DisableEffect(effectInstance);
        

        if (stunActive && CurrentShots <= 0f)
        {
            GetComponent<SlowTowerEffect>().HitBySlow(SlowProc, range, AreaOfEffect, true);
        }
        else
        {
            GetComponent<SlowTowerEffect>().HitBySlow(SlowProc, range, AreaOfEffect, false);
        }

    }

    public override void ShowUpgradeUI(Transform towerMenu)
    {
        for (int i = 0; i < towerMenu.childCount; i++)
        {
            if (towerMenu.GetChild(i).gameObject.name.Equals("UpgradeSlowPanel"))
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
        if (gM.SpendResources(level1Cost,0f))
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
            Level1(sT.gameObject);    
        }        
    }
    protected override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gM.SpendResources(level2Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
            Level2(sT.gameObject);
        }
    }
    protected override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gM.SpendResources(level3Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
            Level3(sT.gameObject);
        }
    }

    protected override void Level1(GameObject tower)
    {
        SlowTower sT = tower.GetComponent<SlowTower>();
        sT.slowRadius += upgradeAmountSlowRadius;
    }

    protected override void Level2(GameObject tower)
    {
        SlowTower sT = tower.GetComponent<SlowTower>();

        GameObject towerUpgradeVisual1 = sT.transform.Find("Container").Find("Level1").gameObject;
        GameObject towerUpgradeVisual2 = sT.transform.Find("Container").Find("Level2").gameObject;

        towerUpgradeVisual1.SetActive(false);
        towerUpgradeVisual2.SetActive(true);

        sT.areaOfEffect = true;
        shots.Clear();
        sT.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    protected override void Level3(GameObject tower)
    {
        SlowTower sT = tower.GetComponent<SlowTower>();

        sT.stunActive = true;

        GameObject towerUpgradeVisual1 = sT.transform.Find("Container").Find("Level2").gameObject;
        GameObject towerUpgradeVisual2 = sT.transform.Find("Container").Find("Level3").gameObject;

        towerUpgradeVisual1.SetActive(false);
        towerUpgradeVisual2.SetActive(true);
    }
} 
