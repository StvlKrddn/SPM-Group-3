using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Unity Setup Fields")]

    [SerializeField] protected string enemyTag = "Enemy";
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected GameObject shot;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject radius;
    protected GameManager gM;
    public GameObject onHitEffect;
    protected TowerUpgradeCotroller tUC;
    public Tower towerScript;
    public GameObject tower;
    public GameObject upgradeUI;
    public GameObject buildUI;

    [Header("BaseStats")]

    public float range = 15f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] private float shotDamage = 5000f;
    public float cost = 150f;
    public float materialCost;

    public float ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    protected Transform target;

    protected Shot bullet;

    public abstract void TypeOfShot(EnemyController enemyTarget);
    public abstract void HitTarget(TowerHitEvent eventInfo);
    public abstract void ShowUpgradeUI(GameObject medium, GameObject infoView);
    /*    public abstract void CheckLevels();*/

    public virtual void TowerLevel1()
    {
        gM = FindObjectOfType<GameManager>();
        tUC = TowerUpgradeCotroller.instance;
    }
    public virtual void TowerLevel2()
    {
        gM = FindObjectOfType<GameManager>();
        tUC = TowerUpgradeCotroller.instance;
    }
    public virtual void TowerLevel3()
    {
        gM = FindObjectOfType<GameManager>();
        tUC = TowerUpgradeCotroller.instance;
    }




    /*    public abstract void HitTarget();*/

    private void Start()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float closestEnemy = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestEnemy)
            {
                closestEnemy = distanceToEnemy; 
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && closestEnemy <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    
    protected void LockOnTarget()
    {
        if (target != null)
        {
            // Lock on target
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
  
        }
    }





    



}
