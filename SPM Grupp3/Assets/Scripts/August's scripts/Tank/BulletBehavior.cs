using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float bulletDamage;
    
    GameObject tank;

    float bulletSpeed;
    float range;
    
    Vector3 originalPosition;

    void Start()
    {
        tank = FindObjectOfType<TankController>().gameObject;
        originalPosition = transform.position;
        bulletSpeed = tank.GetComponent<MachineGun>().BulletSpeed;
        range = tank.GetComponent<MachineGun>().BulletRange;
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