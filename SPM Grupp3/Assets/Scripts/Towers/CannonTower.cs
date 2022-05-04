using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Tower
{
    private float fireCountdown = 0.2f;
    [SerializeField] private float upgradeDamageAmount;
    [SerializeField] private float upgradeRangeAmount;
    [SerializeField] private float upgradeFireRateAmount;
    [SerializeField] private bool shootTwice = false;
    private List<CannonTower> cannonTowers;
    // Start is called before the first frame update
    void Start()
    {
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
/*                if (shootTwice)
                {
                    Shoot();
                }*/
            }

        }
    }

    public override void HitTarget(TowerHitEvent eventInfo)
    {
        print("Hello");
        if (target != null)
        {
            EnemyController enemyTarget = eventInfo.enemyHit.GetComponent<EnemyController>();
            GameObject effectInstance = Instantiate(eventInfo.hitEffect, enemyTarget.transform.position, enemyTarget.transform.rotation);

            Destroy(effectInstance, 1f);
            TypeOfShot(enemyTarget);
            /*Destroy(bullet.gameObject, 2f);*/
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
        enemyTarget.TakeDamage(shotDamage);
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
            if (gO.GetComponent<CannonTower>() != null)
            {
                cannonTowers.Add(gO.GetComponent<CannonTower>());         
            }
        }
    }

    public override void TowerLevel1()
    {
        CheckAllPlacedTowers();
        foreach (CannonTower cT in cannonTowers)
        {
            cT.fireRate += upgradeFireRateAmount;
        }
        fireRate += upgradeFireRateAmount;
        cannonTowers.Clear();
    }
    public override void TowerLevel2()
    {
        CheckAllPlacedTowers();
        foreach (CannonTower cT in cannonTowers)
        {
            cT.shotDamage = upgradeDamageAmount;
        }
        shotDamage = upgradeDamageAmount;
        cannonTowers.Clear();
    }
    public override void TowerLevel3()
    {
        CheckAllPlacedTowers();
        foreach (CannonTower cT in cannonTowers)
        {
            cT.shootTwice = true;
        }
        shootTwice = true;
        cannonTowers.Clear();
    }
}
