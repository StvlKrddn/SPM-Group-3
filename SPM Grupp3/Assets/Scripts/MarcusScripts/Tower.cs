using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Attributes")]
    
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";
    public float turnSpeed = 10f;
    public GameObject shot;
    public Transform firePoint;
    public GameObject radius;
    public GameObject tower;
    
    private Transform target;



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void UpgradeRange()
    {
        range++;
    }

    void UpgradeFireRate()
    {
        fireRate++;
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
    // Start is called before the first frame update
    void Start()
    {
        /*radius.SetActive(false);*/

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation);
        bulletGO.SetActive(true);
        Shot bullet = bulletGO.GetComponent<Shot>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Does this run?");
        if (radius.activeSelf)
        {
            radius.SetActive(false);
        }
        else
        {
            radius.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            // Lock on target
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;           
        }
    }
}
