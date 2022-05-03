using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Transform target;
    public float shotSpeed = 1f;
    private Tower tower;
    private Vector3 direction;
    private float distanceThisFrame;


    private void Awake()
    {
        
    }
    public void Seek(Transform _target)
    {       
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
/*        if (target == null)
        {
            Destroy(gameObject);
            return;
        }*/
        if (target != null)
        {
            distanceThisFrame = shotSpeed * Time.deltaTime;
            direction = target.position - transform.position;

            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        tower = gameObject.GetComponentInParent<Tower>();
        if (other.gameObject.CompareTag("Enemy"))
        {
            EventHandler.Instance.InvokeEvent(new TowerHitEvent(
                    description: "An enemy hit",
                    towerGO: tower.gameObject,
                    hitEffect: tower.onHitEffect,
                    enemyHit: target.gameObject
                    )) ;
            Destroy(gameObject);
        }
    }
}
