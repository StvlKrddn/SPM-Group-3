using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [Space]
    [SerializeField] private float slowProc = 0.7f;
    [SerializeField] private bool areaOfEffect = false;   
    [SerializeField] private float shotsBeforeStun;
    public bool StunActive = false;

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
    private float fireCountdown = 0f;

    private SlowTowerEffect slowTowerEffect;

    [System.NonSerialized] public float CostForUpgrade;

    public float SlowProc { get { return slowProc; } set { slowProc = value; } }
    public float ShotsBeforeStun { get { return shotsBeforeStun; } set { shotsBeforeStun = value; } }
    public float CurrentShots { get { return currentShots; } set { currentShots = value; } }
    public bool AreaOfEffect { get { return areaOfEffect; } set { areaOfEffect = value; } }


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
        SlowTowerEffect slowTowerEffect = GetComponent<SlowTowerEffect>();
        print("vad blir slow toerefefct");

        print(slowTowerEffect);
        
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
                bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation, transform);
                shots.Add(bulletGO);
            }
            else
            {
                bulletGO = shots[shotIndex];
                bulletGO.transform.position = firePoint.position;
                bulletGO.transform.rotation = firePoint.rotation;
            }
            bulletGO.transform.parent = transform;
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
        else
        {
            AOESlow(shotIndex);
        }
    }

    private void  AOESlow(int shotIndex)
    {   
        if (currentShots <= 0 && StunActive)
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
            effectInstance = Instantiate(OnHitEffect, transform.position, transform.rotation, transform);
            shots.Add(effectInstance);
        }
        else
        {
            effectInstance = shots[shotIndex];
            effectInstance.transform.position = transform.position;
            effectInstance.transform.rotation = transform.rotation;
        }
        effectInstance.SetActive(true);

        print(effectInstance);

        StartCoroutine(DisableEffect(effectInstance));

        slowTowerEffect = GetComponent<SlowTowerEffect>();
        print("är slow tower effect null");
        print(slowTowerEffect);

        if (StunActive && CurrentShots <= 0f)
        {   
            slowTowerEffect.HitBySlow(null, SlowProc, range, AreaOfEffect, true);
        }
        else
        {
            slowTowerEffect.HitBySlow(null, SlowProc, range, AreaOfEffect, false);
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
        if (gameManager.SpendResources(level1Cost,0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            SlowTower slowTower = towerManager.ClickedTower.GetComponent<SlowTower>();
            Level1(slowTower.gameObject);    
        }        
    }
    protected override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gameManager.SpendResources(level2Cost, 0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            SlowTower slowTower = towerManager.ClickedTower.GetComponent<SlowTower>();
            Level2(slowTower.gameObject);
        }
    }
    protected override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gameManager.SpendResources(level3Cost, Level3MaterialCost))
        {
            towerManager.IncreaseUpgradesPurchased();
            SlowTower slowTower = towerManager.ClickedTower.GetComponent<SlowTower>();
            Level3(slowTower.gameObject);
        }
    }

    protected override void Level1(GameObject tower)
    {       
        SlowTower slowTower = tower.GetComponent<SlowTower>();
        GetVisualUpgrade(slowTower);
        slowTower.range += upgradeAmountSlowRadius;
        slowTower.Radius.transform.localScale = new Vector3(slowTower.range * 2f, 0.01f, slowTower.range * 2f);      
    }

    protected override void Level2(GameObject tower)
    {
        SlowTower slowTower = tower.GetComponent<SlowTower>();

        level1Visual.SetActive(false);
        level2Visual.SetActive(true);

        slowTower.areaOfEffect = true;
        foreach (GameObject bullet in slowTower.shots)
        {
            Destroy(bullet);
        }
        slowTower.shots.Clear();
        slowTower.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    protected override void Level3(GameObject tower)
    {
        SlowTower slowTower = tower.GetComponent<SlowTower>();

        slowTower.StunActive = true;

        level2Visual.SetActive(false);
        level3Visual.SetActive(true);
    }
} 
