using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower
{
    [SerializeField] private float poisonTicks = 5;
    [SerializeField] private float poisonDamagePerTick = 25;
    [SerializeField] private float upgradeAmountPoisonTicks = 5;
    [SerializeField] private float upgradeAmountPoisonDamagePerTick = 25;
    private float fireCountdown = 0f;

    private List<PoisonTower> poisonTowers = new List<PoisonTower>();

    private bool poisonSpread = false;
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
        
        LockOnTarget();

        if (CanYouShoot())
        {
            Shoot();
        }
    }

    public override void HitTarget(TowerHitEvent eventInfo)
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
    /*    public override void HitTarget()
        {
            if (target != null)
            {
                EnemyController enemyTarget = target.GetComponent<EnemyController>();
    *//*            GameObject effectInstance = Instantiate(onHitEffect, transform.position, transform.rotation);

                Destroy(effectInstance, 1f);*//*
                TypeOfShot(enemyTarget);
                Destroy(bullet.gameObject);
            }
        }*/

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
        print("Running? pois");
        if (poisonSpread)
        {
            enemyTarget.spread = true;
        }
        enemyTarget.HitByPoison(PoisonTicks,PoisonDamagePerTick, onHitEffect);
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

    void CheckAllPlacedTowers()
    {
        foreach (GameObject gO in BuildManager.instance.towersPlaced)
        {
            if (gO.GetComponent<PoisonTower>() != null)
            {
                poisonTowers.Add(gO.GetComponent<PoisonTower>());
            }
        }
    }

    public override void TowerLevel1()
    {
        CheckAllPlacedTowers();
        foreach (PoisonTower pT in poisonTowers)
        {
            pT.poisonTicks += upgradeAmountPoisonTicks;
        }
        poisonTicks += upgradeAmountPoisonTicks;
        poisonTowers.Clear();
    }
    public override void TowerLevel2()
    {
        CheckAllPlacedTowers();
        foreach (PoisonTower pT in poisonTowers)
        {
            pT.poisonDamagePerTick += upgradeAmountPoisonDamagePerTick;
        }
        poisonDamagePerTick += upgradeAmountPoisonDamagePerTick;
        poisonTowers.Clear();
    }
    public override void TowerLevel3()
    {
        CheckAllPlacedTowers();
        foreach (PoisonTower pT in poisonTowers)
        {
            pT.poisonSpread = true;
        }
        poisonSpread = true;
        poisonTowers.Clear();
    }
}
