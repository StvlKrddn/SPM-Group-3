using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoisonTower : Tower
{
    [SerializeField] private float poisonTicks = 5;
    [SerializeField] private float poisonDamagePerTick = 25;
    [SerializeField] private float maxHealthPerTick = 0.1f;
    [SerializeField] private GameObject poisonPulse;

    [Header("Amount To Upgrade")]
    [SerializeField] private float upgradeAmountPoisonTicks = 5;
    [SerializeField] private float upgradeAmountPoisonDamagePerTick = 25;
    [SerializeField] private float upgradeMaxHealthPoisonDamagePerTick = 0.01f;
    [SerializeField] private float upgradeAttackSpeed = 0.1f;
    [Header("Poison Spreading")]
    [SerializeField] private bool poisonSpread = false;

    [Header("Upgrade Cost")]
    [SerializeField] private float level1Cost;
    [SerializeField] private float level2Cost;
    [SerializeField] private float level3Cost; 

    private float fireCountdown = 0f;

    public float costForUpgrade;

    public float PoisonTicks { get { return poisonTicks; } set { poisonTicks = value; } }
    public float PoisonDamagePerTick { get { return poisonDamagePerTick; } set { poisonDamagePerTick = value; } }
    public float MaxHealthPerTick { get { return maxHealthPerTick; } set { maxHealthPerTick = value; } }

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
       // LockOnTarget();

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
                bullet.DecideTypeOfShot("Poison");
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
        GameObject effectInstance = Instantiate(poisonPulse, transform.position, transform.rotation);
        Destroy(effectInstance, 1f);

        //F�r att tornet �r AOE, D� skjuter den inte ut n�t skott
        GetComponent<PoisonTowerEffect>().HitByPoison(PoisonTicks, onHitEffect, PoisonDamagePerTick, MaxHealthPerTick, range);
    }

    public override void ShowUpgradeUI(Transform towerMenu)
    {
        for (int i = 0; i < towerMenu.childCount; i++)
        {
            if (towerMenu.GetChild(i).gameObject.name.Equals("UpgradePoisonPanel"))
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
            PoisonTower pT = tUC.ClickedTower.GetComponent<PoisonTower>();            
            Level1(pT.gameObject);
        }
    }
    protected override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gM.SpendResources(level2Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            PoisonTower pT = tUC.ClickedTower.GetComponent<PoisonTower>();          
            Level2(pT.gameObject);
        }
    }
    protected override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gM.SpendResources(level3Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            PoisonTower pT = tUC.ClickedTower.GetComponent<PoisonTower>();
            Level3(pT.gameObject);
        }
    }

    protected override void Level1(GameObject tower)
    {
        tower.GetComponent<PoisonTower>().poisonTicks += upgradeAmountPoisonTicks;  
    }

    protected override void Level2(GameObject tower)
    {
        PoisonTower pT = tower.GetComponent<PoisonTower>();
        pT.poisonDamagePerTick += upgradeAmountPoisonDamagePerTick;
        pT.maxHealthPerTick += upgradeMaxHealthPoisonDamagePerTick; 
        pT.fireRate += upgradeAttackSpeed;
    }

    protected override void Level3(GameObject tower)
    {
        tower.GetComponent<PoisonTower>().poisonSpread = true;
    }
}
