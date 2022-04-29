using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Transform target;
    public float shotSpeed = 1f;

    public GameObject hitEffect;


    private Tower tower;
    private Vector3 direction;
    private float distanceThisFrame;


    private void Awake()
    {
        tower = gameObject.GetComponentInParent<Tower>();
    }
    public void Seek(Transform _target)
    {       
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        distanceThisFrame = shotSpeed * Time.deltaTime;
        direction = target.position - transform.position;

        if (CheckIfProjectileHit())
        {
            HitTarget();
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    public bool CheckIfProjectileHit()
    {
        if (direction.magnitude <= distanceThisFrame)
        {
            return true;
        }
        return false;
    }

    void HitTarget()
    {
        EnemyController enemyTarget = target.GetComponent<EnemyController>();
        GameObject effectInstance = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 1f);
        
/*        Destroy(target.gameObject);*/
        Destroy(gameObject);
    }


}
