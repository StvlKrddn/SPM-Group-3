using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankState))]
public class WeaponSlot : MonoBehaviour
{

    private TankState tank;
    private Transform turretObject;
    private GameObject spawnedBullet;
    private InputAction shootAction;
    private GameObject bulletPrefab;
    private readonly List<GameObject> bullets = new List<GameObject>();
    private bool allowedToShoot = true;
    [SerializeField] TankWeapon equippedWeapon;

    [SerializeField] private float fireRate;
    [SerializeField] private float spread;
    [SerializeField] private float range;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool penetrating;
    [SerializeField] private float damage;
    [Space]
    [SerializeField] private GameObject muzzleFlash;

    public Transform BulletSpawner;

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

        BulletSpawner = turretObject.Find("BarrelEnd");

        shootAction = tank.PlayerInput.actions["Shoot"];
    }

	private void OnEnable()
	{
        allowedToShoot = true;
	}

	public void ConstructWeapon()
    {
        fireRate = equippedWeapon.fireRate;
        spread = equippedWeapon.spread;
        range = equippedWeapon.range;
        penetrating = equippedWeapon.penetrating;
        bulletSpeed = equippedWeapon.bulletSpeed;
        bulletPrefab = equippedWeapon.bulletPrefab;
        damage = equippedWeapon.damage;
    }

    public void UpgradeShots()
    {
        if (bullets.Count > 0)
        {
            foreach (GameObject bullet in bullets)
            {
                bullet.GetComponent<BulletBehavior>().UpdateBulletStats();
            }
        }
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
        Destroy(Instantiate(muzzleFlash, BulletSpawner.position, BulletSpawner.rotation, BulletSpawner.parent), 0.1f);
        Quaternion spreadDirection = ComputeBulletSpread();
        int bulletIndex = FindShot();
        if (bulletIndex < 0)
        {
            spawnedBullet = Instantiate(
            original: bulletPrefab,
            position: BulletSpawner.position,
            rotation: BulletSpawner.rotation * spreadDirection,
            parent: transform
            );
            bullets.Add(spawnedBullet);
        }
        else
        {
            GameObject bullet = bullets[bulletIndex];
            bullet.transform.SetPositionAndRotation(BulletSpawner.position, BulletSpawner.rotation * spreadDirection);
            bullet.SetActive(true);
        }
    }

    private int FindShot()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].activeSelf == false)
            {
                return i;
            }
        }
        return -1;
    }


    Quaternion ComputeBulletSpread()
    {
        // Produce a random rotation within a certain radius
        Vector3 randomDirection = BulletSpawner.forward + Random.insideUnitSphere * spread;

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
        bulletSpeed += 15;
    }

    public void ClearBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        bullets.Clear();
    }
}
