using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [SerializeField] private float slowProc = 0.7f;
    [SerializeField] private float slowRadius = 3f;
    [SerializeField] private float upgradeAmountSlowProc;
    [SerializeField] private float upgradeAmountSlowRadius;
    private float fireCountdown = 0f;
    /*    [SerializeField] private GameObject shot;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject radius;*/
    public float SlowProc { get { return slowProc; } set { slowProc = value; } }

    private bool singleTarget = true;
    private List<SlowTower> slowTowers = new List<SlowTower>();
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
/*        if (shot != null)
        {

        }*/

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
        print("Running? slow");
        enemyTarget.HitBySlow(SlowProc, slowRadius, singleTarget);
    }
    private void Shoot()
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

    void CheckAllPlacedTowers()
    {
        foreach (GameObject gO in BuildManager.instance.towersPlaced)
        {
            if (gO.GetComponent<SlowTower>() != null)
            {
                slowTowers.Add(gO.GetComponent<SlowTower>());
            }
        }
    }

    public override void TowerLevel1()
    {
        CheckAllPlacedTowers();
        foreach (SlowTower cT in slowTowers)
        {
            cT.singleTarget = false;
        }
        singleTarget = false;
        slowTowers.Clear();
    }
    public override void TowerLevel2()
    {
        CheckAllPlacedTowers();
        foreach (SlowTower cT in slowTowers)
        {
            cT.slowRadius += upgradeAmountSlowRadius;
        }
        slowRadius += upgradeAmountSlowRadius;
        slowTowers.Clear();
    }
    public override void TowerLevel3()
    {
        CheckAllPlacedTowers();
        foreach (SlowTower cT in slowTowers)
        {
            cT.slowProc -= upgradeAmountSlowProc;
        }
        slowProc -= upgradeAmountSlowProc;
        slowTowers.Clear();
    }
}
