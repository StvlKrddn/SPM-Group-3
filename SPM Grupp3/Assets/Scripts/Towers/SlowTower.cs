using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [SerializeField] private float slowProc = 0.7f;
    [SerializeField] private float slowRadius = 1f;
    [SerializeField] private bool areaOfEffect = false;

    [SerializeField] private int shotsBeforeStun;

    [Header("Amount To Upgrade")]
    [SerializeField] private float upgradeAmountSlowProc;
    [SerializeField] private float upgradeAmountSlowRadius;
    [SerializeField] private bool stunEnemiesPeriodically = false;
    [SerializeField] private float stunDuration;
    [SerializeField] private float stunTimer;

    [Header("Upgrade Cost")]
    [SerializeField] private float level1Cost;
    [SerializeField] private float level2Cost;
    [SerializeField] private float level3Cost;

    private int currentShots = 0;

    private bool stunActive = false;



    private float fireCountdown = 0f;
    private float timer;

    public float SlowProc { get { return slowProc; } set { slowProc = value; } }

    

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
            if (stunEnemiesPeriodically)
            {
                if (timer >= stunTimer)
                {
                    StunEnemies();
                }
                timer += Time.deltaTime;
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
             //   GameObject effectInstance = Instantiate(eventInfo.hitEffect, enemyTarget.transform.position, enemyTarget.transform.rotation);

              //  Destroy(effectInstance, 1f);
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
        enemyTarget.HitBySlow(SlowProc, slowRadius, false);
    }
    private void Shoot()
    {   if (!areaOfEffect)
        {
            GameObject bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation);
            bulletGO.transform.parent = transform;
            bulletGO.SetActive(true);
            bullet = bulletGO.GetComponent<Shot>();

            /*        GameObject effectInstance = Instantiate(onHitEffect, transform.position, transform.rotation);
                    Destroy(effectInstance, 1f);*/
            if (bullet != null)
            {
                bullet.Seek(target);
            }
        }
        else
        {
            float amountToSlow;
            if (currentShots <= 0 && stunActive)
            {
                amountToSlow = 0;


                print("kommer den hit");
                currentShots = shotsBeforeStun;
            }
            else
            {
                 amountToSlow = slowProc;

                currentShots -= 1; 
            }

            

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
                    enemyontroller.HitBySlow(amountToSlow, slowRadius, false);
                    //EnemyController enemyTarget = eventInfo.enemyHit.GetComponent<EnemyController>();



                    //   TypeOfShot(enemyontroller);
                }
            }
        }
    }

    private void StunEnemies()
    {
/*        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.GetComponent<EnemyController>())
            {
                c.GetComponent<EnemyController>().TakeDamage(splashDamage);
            }
        }*/
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

    public override void TowerLevel1()
    {
        base.TowerLevel1();
        if (gM.SpendResources(level1Cost,0f))
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
            sT.slowRadius += upgradeAmountSlowRadius;
                         
        }        
    }
    public override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gM.SpendResources(level2Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
            sT.areaOfEffect = true;
            sT.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    
        
    }
    public override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gM.SpendResources(level3Cost, 0f))
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
            sT.stunActive = true; 
        }
    }

}
