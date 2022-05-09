using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [SerializeField] private float slowProc = 0.7f;
    [SerializeField] private float slowRadius = 3f;

    [Header("Amount To Upgrade")]
    [SerializeField] private float upgradeAmountSlowProc;
    [SerializeField] private float upgradeAmountSlowRadius;

    [Header("Upgrade Cost")]
    [SerializeField] private float level1Cost;
    [SerializeField] private float level2Cost;
    [SerializeField] private float level3Cost;

    [Header("Purchased Upgrades")]
    [SerializeField] private bool level1UpgradePurchased = false;
    [SerializeField] private bool level2UpgradePurchased = false;
    [SerializeField] private bool level3UpgradePurchased = false;

    private float fireCountdown = 0f;
    private bool singleTarget = true;
    private List<SlowTower> slowTowers = new List<SlowTower>();

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

    public override void ShowUpgradeUI(GameObject medium, GameObject infoView)
    {
        if (infoView.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            infoView.transform.GetChild(2).gameObject.SetActive(false);
            medium.SetActive(true);
        }
        else
        {
            infoView.transform.GetChild(2).gameObject.SetActive(true);
            medium.SetActive(false);
        }
    }

    public override void TowerLevel1()
    {
        base.TowerLevel1();
        if (gM.SpendResources(level1Cost,0f) && !level1UpgradePurchased)
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();           
            sT.singleTarget = false;              
        }        
    }
    public override void TowerLevel2()
    {
        base.TowerLevel2();
        if (gM.SpendResources(level2Cost, 0f) && !level2UpgradePurchased && level1UpgradePurchased)
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
            sT.slowRadius += upgradeAmountSlowRadius;
        }
    }
    public override void TowerLevel3()
    {
        base.TowerLevel3();
        if (gM.SpendResources(level3Cost, 0f) && !level3UpgradePurchased && level2UpgradePurchased && level1UpgradePurchased)
        {
            tUC.IncreaseUpgradesPurchased();
            SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
            sT.slowProc -= upgradeAmountSlowProc;
        }
    }

}
