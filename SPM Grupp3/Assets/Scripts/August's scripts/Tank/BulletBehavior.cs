using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private float bulletSpeed;
    private float range;
    [SerializeField] private GameObject tank;
    [SerializeField] private float bulletDamage;
    
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        bulletSpeed = tank.GetComponent<TankController>().BulletSpeed;
        range = tank.GetComponent<TankController>().BulletRange;
    }

    void Update()
    {
        Vector3 relativePosition = originalPosition - transform.position;

        if (relativePosition.magnitude < range)
        {
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float BulletDamage { get { return bulletDamage; } set { bulletDamage = value; } }
}
