using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Attributes")]
    
    [SerializeField] public float range = 15f;
    [SerializeField] protected float fireRate = 1f;

    public float cost = 150f;

    public float materialCost;
    [SerializeField] protected float shotDamage = 5000f;

    [Header("Unity Setup Fields")]

    [SerializeField] protected string enemyTag = "Enemy";
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected GameObject shot;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject radius;
    public GameObject onHitEffect;

    public Tower towerScript;
    public GameObject tower;
    public LayerMask towers;
/*    public GameObject upgradeUI;*/
    protected Transform target;

    protected float ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    protected Shot bullet;

    public abstract void TypeOfShot(EnemyController enemyTarget);
    public abstract void HitTarget(TowerHitEvent eventInfo);
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
