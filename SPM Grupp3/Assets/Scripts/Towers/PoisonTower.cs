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

    private PoisonTowerEffect poisonTowerEffect;

    [System.NonSerialized] public float CostForUpgrade;


    public float PoisonTicks { get { return poisonTicks; } set { poisonTicks = value; } }
    public float PoisonDamagePerTick { get { return poisonDamagePerTick; } set { poisonDamagePerTick = value; } }
    public float MaxHealthPerTick { get { return maxHealthPerTick; } set { maxHealthPerTick = value; } }

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

    void Start()
    {
        Radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        poisonTowerEffect = GetComponent<PoisonTowerEffect>();


    }

    void Update()
    {     
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
            if (poisonSpread)
            {
                enemyTarget.Spread = true;
            }
            bullet.DecideTypeOfShot("Poison");

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
        GameObject effectInstance;
        int bulletIndex = FindShot();
        if (bulletIndex < 0)
        {
            effectInstance = Instantiate(poisonPulse, transform.position, transform.rotation, transform);
            shots.Add(effectInstance);
        }
        else
        {
            effectInstance = shots[bulletIndex];
            effectInstance.transform.position = transform.position;
            effectInstance.transform.rotation = transform.rotation;
        }
        effectInstance.SetActive(true);
        StartCoroutine(DisableEffect(effectInstance));

        //F�r att tornet �r AOE, D� skjuter den inte ut n�t skott
        poisonTowerEffect.HitByPoison(PoisonTicks, OnHitEffect, PoisonDamagePerTick, MaxHealthPerTick, range);
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
        if (gameManager.SpendResources(level1Cost, 0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            PoisonTower poisonTower = towerManager.ClickedTower.GetComponent<PoisonTower>();            
            Level1(poisonTower.gameObject);
        }
    }
    protected override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gameManager.SpendResources(level2Cost, 0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            PoisonTower poisonTower = towerManager.ClickedTower.GetComponent<PoisonTower>();          
            Level2(poisonTower.gameObject);
        }
    }
    protected override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gameManager.SpendResources(level3Cost, 0f))
        {
            towerManager.IncreaseUpgradesPurchased();
            PoisonTower poisonTower = towerManager.ClickedTower.GetComponent<PoisonTower>();
            Level3(poisonTower.gameObject);
        }
    }

    protected override void Level1(GameObject tower)
    {
        PoisonTower poisonTower = tower.GetComponent<PoisonTower>();

        poisonTower.poisonTicks += upgradeAmountPoisonTicks;
        GetVisualUpgrade(poisonTower);

        level1Visual.transform.localScale = new Vector3(level1Visual.transform.localScale.x *1.2f, level1Visual.transform.localScale.y * 1.2f, level1Visual.transform.localScale.z * 1.2f);
    }

    protected override void Level2(GameObject tower)
    {
        PoisonTower poisonTower = tower.GetComponent<PoisonTower>();
        poisonTower.poisonDamagePerTick += upgradeAmountPoisonDamagePerTick;
        poisonTower.maxHealthPerTick += upgradeMaxHealthPoisonDamagePerTick; 
        poisonTower.fireRate += upgradeAttackSpeed;
       
        level1Visual.SetActive(false);
        level2Visual.SetActive(true);
    }

    protected override void Level3(GameObject tower)
    {
        PoisonTower poisonTower = tower.GetComponent<PoisonTower>();
        poisonTower.poisonSpread = true;

        level2Visual.SetActive(false);
        level3Visual.SetActive(true);
    }


}
