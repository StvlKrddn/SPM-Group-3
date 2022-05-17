using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] protected float damage;
    public bool penetrating;
    
    GameObject tank;

    public float bulletSpeed;
    public float range;
    
    Vector3 originalPosition;
    private int penetrationCount = 3;
    
    public float BulletDamage { get { return damage; } set { damage = value; } }
    public float BulletRange { get { return range; } set { range = value; } }

    void Start()
    {
        // NOTE(August): Ändra från FindObjectOfType eftersom den körs varje gång en kula skjuts
        tank = GetComponentInParent<TankState>().gameObject;
        originalPosition = transform.position;
        bulletSpeed = tank.GetComponent<WeaponSlot>().BulletSpeed;
        range = tank.GetComponent<WeaponSlot>().BulletRange;
        damage = tank.GetComponent<WeaponSlot>().BulletDamage;
        penetrating = tank.GetComponent<WeaponSlot>().BulletPenetration;
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
