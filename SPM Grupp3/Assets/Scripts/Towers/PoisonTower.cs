using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoisonTower : Tower
{
    [SerializeField] private float poisonTicks = 5;
    [SerializeField] private float poisonDamagePerTick = 25;
    [SerializeField] private float maxHealthPerTick = 0.1f;

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


    public float PoisonTicks { get { return poisonTicks; } set { poisonTicks = value; } }
    public float PoisonDamagePerTick { get { return poisonDamagePerTick; } set { poisonDamagePerTick = value; } }

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
        if (poisonSpread)
        {
            enemyTarget.spread = true;
        }
        //enemyTarget.HitByPoison(PoisonTicks,PoisonDamagePerTick, onHitEffect, maxHealthPerTick);
    }

    protected void Shoot()
    {
        
        //     GameObject bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation);
        //     bulletGO.transform.parent = transform;
        //     bulletGO.SetActive(true);
        //     bullet = bulletGO.GetComponent<Shot>();
        //
        //     if (bullet != null)
        //     {
        //         bullet.Seek(target);
        //     }
        

        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider c in colliders)
        {
            if (c.GetComponent<EnemyController>())
            {
                GameObject effectInstance = Instantiate(onHitEffect, transform.position, transform.rotation);

                ParticleSystem particleEffect = effectInstance.GetComponent<ParticleSystem>();
                   
            //    particleEffect.SetCustomParticleData = 5; //= radius;

                Destroy(effectInstance, 1f);
                EnemyController enemyontroller = c.GetComponent<EnemyController>();
                enemyontroller.HitByPoison(PoisonTicks, PoisonDamagePerTick, maxHealthPerTick);
                //EnemyController enemyTarget = eventInfo.enemyHit.GetComponent<EnemyController>();
                

                
             //   TypeOfShot(enemyontroller);
            }
        }
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

    public override void TowerLevel1()
    {
        base.TowerLevel1();
        if (gM.SpendResources(level1Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            PoisonTower pT = tUC.ClickedTower.GetComponent<PoisonTower>();            
            pT.poisonTicks += upgradeAmountPoisonTicks;                
        }
    }
    public override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gM.SpendResources(level2Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            PoisonTower pT = tUC.ClickedTower.GetComponent<PoisonTower>();          
            pT.poisonDamagePerTick += upgradeAmountPoisonDamagePerTick;
            pT.maxHealthPerTick += upgradeMaxHealthPoisonDamagePerTick; 
            pT.fireRate += upgradeAttackSpeed;
        }
    }
    public override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gM.SpendResources(level3Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            PoisonTower pT = tUC.ClickedTower.GetComponent<PoisonTower>();
            pT.poisonSpread = true;               
        }
    }
}
