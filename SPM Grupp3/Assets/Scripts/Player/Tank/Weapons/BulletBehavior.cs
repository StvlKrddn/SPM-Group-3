using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] protected float damage;
    public bool penetrating;

    private WeaponSlot weapon;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float range;
    [SerializeField] private GameObject muzzleFlash;
    
    Vector3 originalPosition;
    [SerializeField] private int penetrationCount = 2;
    private int penetrationCountMax;
    

    public float BulletDamage { get { return damage; } set { damage = value; } }
    public float BulletRange { get { return range; } set { range = value; } }

	private void Awake()
	{
        penetrationCountMax = penetrationCount;
	}

	void Start()
    {
        GameObject tank = GetComponentInParent<TankState>().gameObject;
        weapon = tank.GetComponent<WeaponSlot>();
        UpdateBulletStats();
        transform.parent = null;
        originalPosition = transform.position;

    }

	private void OnEnable()
	{
        originalPosition = transform.position;
        penetrationCount = penetrationCountMax;
	}

	protected virtual void OnBecameInvisible()
	{
        gameObject.SetActive(false);
    }

	public void UpdateBulletStats()
    {
        bulletSpeed = weapon.BulletSpeed;
        range = weapon.BulletRange;
        damage = weapon.BulletDamage;
        penetrating = weapon.BulletPenetration; //The bullet gets the stats from their weapon
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
            gameObject.SetActive(false);
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
                gameObject.SetActive(false);
            }
        }
    }
}
