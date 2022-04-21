using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankController))]
public class MachineGun : MonoBehaviour, ITankWeapon
{
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletSpread = 20f;
    [SerializeField] private float bulletRange = 20f;
    [SerializeField] private float bulletSpeed = 35f;

    [Header("Bullet prefab: ")]
    [SerializeField] private GameObject bulletPrefab;

    TankState tank;
    Transform bulletSpawner;
    BulletBehavior bullet;
    Transform turretObject;
    GameObject spawnedBullet;
    InputAction shootAction;

    bool allowedToShoot = true;

    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float BulletSpread { get { return bulletSpread; } set { bulletSpread = value; } }
    public float BulletRange { get { return bulletRange; } set { bulletRange = value; } }
    public float BulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }

    void Start()
    {
        tank = GetComponent<TankState>();

        bulletSpread = Mathf.Clamp(bulletSpread, 0, 50);

        turretObject = transform.GetChild(0);

        bulletSpawner = turretObject.Find("BarrelEnd");

        shootAction = tank.PlayerInput.actions["Shoot"];
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
        Vector3 randomDirection = bulletSpawner.forward + Random.insideUnitSphere * bulletSpread;

        // Prevent too much spread up and down
        randomDirection = new Vector3(Mathf.Clamp01(randomDirection.x), randomDirection.y, randomDirection.z);

        return Quaternion.Euler(randomDirection);
    }

    public void UpgradeFirerate(float modifier)
    {
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
}
