using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankState))]
public class WeaponSlot : MonoBehaviour
{

    [SerializeField] TankWeapon equippedWeapon;

    private float fireRate;
    private float spread;
    private float range;
    private float bulletSpeed;
    private bool penetrating;
    private GameObject bulletPrefab;

    TankState tank;
    Transform bulletSpawner;
    BulletBehavior bullet;
    Transform turretObject;
    GameObject spawnedBullet;
    InputAction shootAction;

    bool allowedToShoot = true;

    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float BulletSpread { get { return spread; } set { spread = value; } }
    public float BulletRange { get { return range; } set { range = value; } }
    public float BulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }

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

    void ConstructWeapon()
    {
        fireRate = equippedWeapon.fireRate;
        spread = equippedWeapon.spread;
        range = equippedWeapon.range;
        penetrating = equippedWeapon.penetrating;
        bulletSpeed = equippedWeapon.bulletSpeed;
        bulletPrefab = equippedWeapon.bulletPrefab;
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
        yield return new WaitForSeconds(fireRate);
        allowedToShoot = true;
    }

    void SpawnBullet()
    {
        Quaternion spreadDirection = ComputeBulletSpread();

        spawnedBullet = Instantiate(
            original: bulletPrefab,
            position: bulletSpawner.position,
            rotation: bulletSpawner.rotation * spreadDirection
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
        //1
        fireRate += modifier;
    }

    public void UpgradeDamage(float modifier)
    {
        bullet.BulletDamage += modifier;
    }

    public void UpgradeRange(float modifier)
    {
        bullet.BulletRange += modifier;
    }

    public void UpgradePenetration()
    {
        penetrating = true;
    }
}
