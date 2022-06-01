using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private int penetrationCountMax;
    private WeaponSlot weapon;
    private Vector3 originalPosition;
    private Material material;
    private bool sniperShot;
    private readonly float sniperScale = 1.5f;
    [SerializeField] private int penetrationCount = 2;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float range;
    [SerializeField] protected float damage;
    [SerializeField] private bool penetrating;

    public float BulletDamage { get { return damage; } set { damage = value; } }
    public float BulletRange { get { return range; } set { range = value; } }

	protected virtual void Start()
    {
        GameObject tank = GetComponentInParent<TankState>().gameObject;
        weapon = tank.GetComponent<WeaponSlot>();
        if (GetComponent<Renderer>())
        {
            material = GetComponent<Renderer>().material;
        }
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
        penetrationCountMax = weapon.BulletPenetrationCount;
        if (material != null && sniperShot != true)
        {
            sniperShot = true;
            transform.localScale *= sniperScale;
            material.SetColor("_EmissionColor", weapon.BulletColor);
        }
    }


    void Update()
    {
        Vector3 relativePosition = originalPosition - transform.position;

        if (relativePosition.magnitude < range)
        {
            transform.position += bulletSpeed * Time.deltaTime * transform.forward;
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
