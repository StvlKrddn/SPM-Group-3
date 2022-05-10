using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float bulletDamage;
    public bool penetrating;
    
    GameObject tank;

    float bulletSpeed;
    float range;
    
    Vector3 originalPosition;
    private int penetrationCount = 3;
    
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
        if (other.CompareTag("Enemy"))
        {
            if (penetrating == true && penetrationCount > 0)
            {
                penetrationCount--;
            }
            else
            {
				Destroy(gameObject, 0.01f);
            }
        }
    }
}
