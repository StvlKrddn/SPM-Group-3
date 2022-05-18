using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] protected float damage;
    public bool penetrating;
    
    private GameObject tank;
    private WeaponSlot weapon;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float range;
    
    Vector3 originalPosition;
    [SerializeField] private int penetrationCount = 3;
    
    public float BulletDamage { get { return damage; } set { damage = value; } }
    public float BulletRange { get { return range; } set { range = value; } }

    void Start()
    {
        // NOTE(August): Ändra från FindObjectOfType eftersom den körs varje gång en kula skjuts
        tank = GetComponentInParent<TankState>().gameObject;
        weapon = tank.GetComponent<WeaponSlot>();
        originalPosition = transform.position;
        bulletSpeed = weapon.BulletSpeed;
        range = weapon.BulletRange;
        damage = weapon.BulletDamage;
        penetrating = weapon.BulletPenetration; //The bullet gets the stats from their weapon
        transform.parent = null;
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (penetrating == true && penetrationCount > 0) // Penetrating goes through the enemy
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
