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
    public LayerMask towers;
    
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
        radius.transform.localScale = new Vector3(range * 2f,0.01f,range * 2f);
        radius.SetActive(false);

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



    RaycastHit CastRayFromCamera(LayerMask layerMask)
    {
        // Get mouse position
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from camera to mouse position
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition);

        // Raycast along the ray and return the hit point
        Physics.Raycast(ray: cameraRay, hitInfo: out RaycastHit hit, maxDistance: Mathf.Infinity, layerMask: layerMask);

        return hit;
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

        if (Input.GetMouseButtonDown(0))
        {
            GameObject towerHit = GetTower();

            if (towerHit != null && tower == towerHit.transform.parent)
            {
                if (radius.activeSelf)
                {
                    radius.SetActive(true);
                }
                else
                {
                    radius.SetActive(false);
                }
            }
        }
    }

    GameObject GetTower()
    {
        RaycastHit hit = CastRayFromCamera(towers);

        // If a tower was hit, return the tower
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}
