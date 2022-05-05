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
    public GameObject onHitEffect;
    
    public Tower towerScript;
    public GameObject tower;

    [Header("BaseStats")]

    [SerializeField] protected float range = 15f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] private float shotDamage = 5000f;
    public float cost = 150f;
    public float materialCost;
    

    public float ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    protected Transform target;
    protected GameManager gM;
    protected Shot bullet;

    public abstract void TypeOfShot(EnemyController enemyTarget);
    public abstract void HitTarget(TowerHitEvent eventInfo);

    public abstract void TowerLevel1();
    public abstract void TowerLevel2();
    public abstract void TowerLevel3();

    /*    public abstract void HitTarget();*/

    private void Start()
    {
        gM = FindObjectOfType<GameManager>();
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
