// Används till att utnyttja livsystemet till pansarvagnar
using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{

    // Inspector variables
    [Header("Movement properties")]
    [SerializeField] private float movementSpeed = 6f;

    // Flyttat attributen och mekaniken till en enklid script
//    [Header("Health")]
//    [SerializeField] private float health = 50f;

    [Header("Shooting properties")]
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletSpread = 35f;
    [SerializeField] private float bulletRange = 20f;
    [SerializeField] private float bulletSpeed = 35f;

    [Header("Boost properties")]
    [SerializeField] private float boostSpeedMultiplier = 3f;
    [SerializeField] private float boostDuration = 1f;
    [SerializeField] private float boostCooldownTime = 5f;

    [Header("Random components: ")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform garage;
    [SerializeField] private GameManager gameManager;

    // Component för att kunna aktivera menyn när fordonet triggar 
    [SerializeField] private GameObject BuildView;

    // Components
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Transform bulletSpawner;
    private Transform turretObject;

    // Caching input actions
    private InputAction moveGamepadAction;
    private InputAction aimAction;
    private InputAction boostAction;
    private InputAction shootAction;

    // Private variables
    private float playerID;
    private Vector2 gamepadInputVector;
    private Vector3 aimInputVector;
    private float aimSpeed;
    private bool allowedToShoot = true;
    private bool allowedToBoost = true;
    private bool allowedToMove = true;
    private float boostTimer;
    private float boostAccelerationTimeMultiplier = 8f;
    private float speedBeforeBoost;
    private float bulletSpreadBeforeBoost;
    private float bulletSpreadDuringBoostMultiplier = 2f;
    private float bulletSpreadIncreaseMultiplier = 10f;
    private Matrix4x4 isoMatrix;

    // Getters and Setters
    public float MovementSpeed
    {
        get { return movementSpeed; }

        // Any time movement speed is altered from another script, it updates the speedBeforeBoost value to reflect the new speed
        set { movementSpeed = value; speedBeforeBoost = value; }
    }
    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float BulletSpread { get { return bulletSpread; } set { bulletSpread = value; } }
    public float BulletRange { get { return bulletRange; } set { bulletRange = value; } }
    public float BulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }
    public float BoostSpeedMultiplier { get { return boostSpeedMultiplier; } set { boostSpeedMultiplier = value; } }
    public float BoostDuration { get { return boostDuration; } set { boostDuration = value; } }
    public float BoostCooldownTime { get { return boostCooldownTime; } set { boostCooldownTime = value; } }

    void Awake()
    {
        InitializeInputSystem();

        rb = GetComponent<Rigidbody>();

        turretObject = transform.GetChild(0);
        bulletSpawner = turretObject.Find("BarrelEnd");

        aimSpeed = movementSpeed * 5;

        speedBeforeBoost = movementSpeed;
        bulletSpreadBeforeBoost = bulletSpread;

        bulletSpread = Mathf.Clamp(bulletSpread, 0, 60);

        gameManager.OnNewWave += OnNewWave;

        // Kontrollerar att BuildView alltid är inaktiverad vid start på rond
        BuildView.active = false;

        //Create isometric matrix
        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        /* Eplanation of isometric translation can be found here: https://youtu.be/8ZxVBCvJDWk */
    }

    void InitializeInputSystem()
    {
        playerInput = GetComponent<PlayerInput>();

        playerID = playerInput.playerIndex;

        moveGamepadAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
        boostAction = playerInput.actions["Boost"];
        shootAction = playerInput.actions["Shoot"];
    }

    void Update()
    {
        gamepadInputVector = moveGamepadAction.ReadValue<Vector2>();
        aimInputVector = aimAction.ReadValue<Vector2>();

        Boost();
        RotateTurret();

        if (shootAction.IsPressed() && allowedToShoot)
        {
            StartCoroutine(Shoot());
        }
            
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Reform the input vector to the right plane
        Vector3 movementVector = new Vector3(gamepadInputVector.x, 0, gamepadInputVector.y);

        Vector3 skewedVector = TranslateToIsometric(movementVector);
        
        Vector3 movement = skewedVector * movementSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);

        if (moveGamepadAction.IsPressed())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(skewedVector), Time.deltaTime * movementSpeed);
        }
    }

    void RotateTurret()
    {
        Vector3 aimVector = new Vector3(aimInputVector.x, 0, aimInputVector.y);

        Vector3 skewedVector = TranslateToIsometric(aimVector);

        if (aimAction.IsPressed())
        {
            turretObject.rotation = Quaternion.Slerp(turretObject.rotation, Quaternion.LookRotation(skewedVector), Time.deltaTime * aimSpeed);
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
        
        GameObject spawnedBullet = Instantiate(
            original: bullet, 
            position: bulletSpawner.position, 
            rotation: bulletSpawner.rotation * spreadDirection
            );
    }

    Quaternion ComputeBulletSpread()
    {
        // Produce a random rotation within a certain radius
        // Då System packet använder en annan Random kod förtydligas detta /Noah
        Vector3 randomDirection = bulletSpawner.forward + UnityEngine.Random.insideUnitSphere * bulletSpread;

        // Prevent too much spread up and down
        randomDirection = new Vector3(Mathf.Clamp01(randomDirection.x), randomDirection.y, randomDirection.z);

        return Quaternion.Euler(randomDirection);
    }

    void Boost()
    {
        if (boostAction.IsPressed() && allowedToBoost)
        {
            StartCoroutine(BoostCooldown());
        
            boostTimer = boostDuration;
        }
        
        // If the boost timer is not yet finished
        if (boostTimer > 0f)
        {
            // Subtract elapsed time since last frame from timer
            boostTimer -= Time.deltaTime;

            // Multiply movement speed
            movementSpeed = Mathf.Lerp(movementSpeed, speedBeforeBoost * boostSpeedMultiplier, Time.deltaTime * boostAccelerationTimeMultiplier);

            // Increase bullet spread during boost
            bulletSpread = Mathf.Lerp(bulletSpread, bulletSpreadBeforeBoost * bulletSpreadDuringBoostMultiplier, Time.deltaTime * bulletSpreadIncreaseMultiplier);
        }
        else
        {
            // Reset movement speed and bullet spread
            movementSpeed = Mathf.Lerp(movementSpeed, speedBeforeBoost, Time.deltaTime * boostAccelerationTimeMultiplier);
            bulletSpread = Mathf.Lerp(bulletSpread, bulletSpreadBeforeBoost, Time.deltaTime * bulletSpreadIncreaseMultiplier);
        }
    }

    IEnumerator BoostCooldown()
    {
        allowedToBoost = false;
        yield return new WaitForSeconds(boostCooldownTime);
        allowedToBoost = true;
    }

    Vector3 TranslateToIsometric(Vector3 vector)
    {
        // Skewer the input vector 45 degrees to accomodate for the isometric perspective
        return isoMatrix.MultiplyPoint3x4(vector);
    }

    /* // Koden nedan har flyttats till Health-Script för mer specifik sortering
        private void OnTriggerEnter(Collider other)
        {
                    if (other.gameObject.CompareTag("EnemyBullet"))
                    {
                        EnemyBullet enemyBullet = other.gameObject.GetComponent<EnemyBullet>();
                        health -= enemyBullet.damage;
                        if (health < 0)
                        {
                            print("Tank destroyed!");
                            MoveToGarage();
                        }
                        Destroy(enemyBullet);
                    }
        }
    */

    // Aktivera BuildView för personliga BuildMode för spelaren 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Garage"))
        {
            BuildView.active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Garage"))
        {
            BuildView.active = false;
        }
    }

    public void MoveToGarage()
    {
        transform.position = garage.position;
        allowedToMove = false;
    }

    void OnNewWave()
    {
        allowedToMove = true;
    }

}
