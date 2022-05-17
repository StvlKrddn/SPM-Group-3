using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankState))]
public class WeaponSlot : MonoBehaviour
{

    [SerializeField] TankWeapon equippedWeapon;

    [SerializeField] private float fireRate;
    [SerializeField] private float spread;
    [SerializeField] private float range;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool penetrating;
    [SerializeField] private float damage;
    private GameObject bulletPrefab;

    TankState tank;
    public Transform bulletSpawner;
    BulletBehavior bullet;
    Transform turretObject;
    GameObject spawnedBullet;
    InputAction shootAction;

    bool allowedToShoot = true;

    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float BulletSpread { get { return spread; } set { spread = value; } }
    public float BulletRange { get { return range; } set { range = value; } }
    public float BulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }
    public float BulletDamage { get { return damage; } set { damage = value; } }
    public bool BulletPenetration { get { return penetrating; } set { penetrating = value; } }

    void Start()
    {
        if (equippedWeapon != null){
            ConstructWeapon();
        }
        
        tank = GetComponent<TankState>();

        spread = Mathf.Clamp(spread, 0, 50);

        turretObject = transform.GetChild(0);

        bulletSpawner = turretObject.Find("BarrelEnd");

        shootAction = tank.PlayerInput.actions["Shoot"];
    }

	private void OnEnable()
	{
        allowedToShoot = true;
	}

	void ConstructWeapon()
    {
        fireRate = equippedWeapon.fireRate;
        spread = equippedWeapon.spread;
        range = equippedWeapon.range;
        penetrating = equippedWeapon.penetrating;
        bulletSpeed = equippedWeapon.bulletSpeed;
        bulletPrefab = equippedWeapon.bulletPrefab;
        damage = equippedWeapon.damage;
		bullet = bulletPrefab.GetComponent<BulletBehavior>();
    }

	private void OnEnable()
	{
        allowedToShoot = true;
	}

	void Update()
    {
        if (shootAction.IsPressed() && allowedToShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        allowedToShoot = false;
        SpawnBullet();
        yield return new WaitForSeconds(1 / fireRate);
        allowedToShoot = true;
    }

    void SpawnBullet()
    {
        Quaternion spreadDirection = ComputeBulletSpread();

        spawnedBullet = Instantiate(
            original: bulletPrefab,
            position: bulletSpawner.position,
            rotation: bulletSpawner.rotation * spreadDirection,
            parent: transform
            );
    }


    Quaternion ComputeBulletSpread()
    {
        // Produce a random rotation within a certain radius
        Vector3 randomDirection = bulletSpawner.forward + Random.insideUnitSphere * spread;

        // Prevent too much spread up and down
        randomDirection = new Vector3(Mathf.Clamp01(randomDirection.x), randomDirection.y, randomDirection.z);

        return Quaternion.Euler(randomDirection);
    }

    public void UpgradeFirerate(float modifier)
	{
        fireRate = modifier;
    }

    public void MaxRange()
    {
        range = 100;
        penetrating = true;
    }

	public void MakeSniper(float range, float fireRateMultiply, float damageIncrease)
	{
		fireRate = fireRateMultiply;
        damage = damageIncrease;
        this.range = range;
        spread = 0;
	}
}
