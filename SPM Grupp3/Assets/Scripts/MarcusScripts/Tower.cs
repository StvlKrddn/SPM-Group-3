using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Attributes")]
    
    [SerializeField] protected float range = 15f;
    [SerializeField] protected float fireRate = 1f;
    public float cost = 150f;
    [SerializeField] protected float fireCountdown = 0f;
    public float materialCost;
    [SerializeField] protected float shotDamage = 5000f;

    [Header("Unity Setup Fields")]

    [SerializeField] protected string enemyTag = "Enemy";
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected GameObject shot;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject radius;

    public GameObject tower;
    public LayerMask towers;
/*    public GameObject upgradeUI;*/
    protected Transform target;

    protected float ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    protected Shot bullet;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    /*    private void OnMouseDown()
        {    
            if (clicked == false)
            {
                radius.SetActive(true);
                upgradeUI.SetActive(true);
                *//*placedUI = Instantiate(upgradeUI, gameObject.transform);*//*
                clicked = true;
            }
            else
            {
                radius.SetActive(false);
                upgradeUI.SetActive(false);
                *//*Destroy(placedUI);*//*
                clicked = false;
            }       
        }*/

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


    protected bool CanYouShoot()
    {
        if (fireCountdown <= 0f)
        {          
            fireCountdown = 1f / fireRate;
            return true;
        }
        fireCountdown -= Time.deltaTime;
        return false;
    }


    protected abstract void TypeOfShot(EnemyController enemyTarget);


    /*    RaycastHit CastRayFromCamera(LayerMask layerMask)
        {
            // Get mouse position
            Vector3 mousePosition = Input.mousePosition;

            // Create a ray from camera to mouse position
            Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition);

            // Raycast along the ray and return the hit point
            Physics.Raycast(ray: cameraRay, hitInfo: out RaycastHit hit, maxDistance: Mathf.Infinity, layerMask: layerMask);

            return hit;
        }*/

    /*    GameObject GetTower()
        {
            RaycastHit hit = CastRayFromCamera(towers);

            // If a tower was hit, return the tower
            return hit.collider != null ? hit.collider.gameObject : null;
        }*/

}
