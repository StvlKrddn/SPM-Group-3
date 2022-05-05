using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float bulletDamage;
    [SerializeField] private bool penetrating;
    
    GameObject tank;

    float bulletSpeed;
    float range;
    
    Vector3 originalPosition;
    
    public float BulletDamage { get { return bulletDamage; } set { bulletDamage = value; } }
    public float BulletRange { get { return range; } set { range = value; } }

    void Start()
    {
        // NOTE(August): Ändra från FindObjectOfType eftersom den körs varje gång en kula skjuts
        tank = FindObjectOfType<TankState>().gameObject;
        originalPosition = transform.position;
        bulletSpeed = tank.GetComponent<WeaponSlot>().BulletSpeed;
        range = tank.GetComponent<WeaponSlot>().BulletRange;
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

    private void OnTriggerEnter(Collider other)
    {
        if (!penetrating && other.CompareTag("Enemy"))
        {
            Destroy(gameObject, 0.01f);
        }
    }
}
