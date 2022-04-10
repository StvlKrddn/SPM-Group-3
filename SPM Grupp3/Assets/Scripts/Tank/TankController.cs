using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    // NOTE(August): OM DEN RÖR PÅ SIG KONSTIGT KAN DET BERO PÅ ATT ROTATIONEN ÄR LÅST PÅ RIGIDBODY

    [Header("Movement properties")]
    [SerializeField] private float movementSpeed = 5;

    [Header("Shooting properties")]
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletSpread = 50f;
    [SerializeField] private float bulletRange = 20f;
    [SerializeField] private float bulletSpeed = 35f;

    [Header("Boost properties")]
    [SerializeField] private float boostSpeedMultiplier = 2f;
    [SerializeField] private float boostDuration = 1f;
    [SerializeField] private float boostCooldownTime = 5f;

    [Header("Bullet prefab: ")]
    [SerializeField] private GameObject bullet;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Transform bulletSpawner;

    // Caching input actions
    private InputAction moveAction;
    private InputAction moveGamepadAction;
    private InputAction boostAction;
    private InputAction shootAction;

    private float turnSpeed;
    private Vector2 movementInputVector;
    private Vector2 gamepadInputVector;
    private bool allowedToShoot = true;
    private bool allowedToBoost = true;
    private float boostTimer;
    private float speedBeforeBoost;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        bulletSpawner = transform.Find("BarrelEnd");

        InitializeInputSystem();

        turnSpeed = movementSpeed / 1.5f;

        speedBeforeBoost = movementSpeed;

        bulletSpread = Mathf.Clamp(bulletSpread, 0, 60);
    }

    void InitializeInputSystem()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        moveGamepadAction = playerInput.actions["Move Gamepad"];
        boostAction = playerInput.actions["Boost"];
        shootAction = playerInput.actions["Shoot"];
    }

    void Update()
    {
        movementInputVector = moveAction.ReadValue<Vector2>();
        gamepadInputVector = moveGamepadAction.ReadValue<Vector2>();
        
        
        
        Boost();

        if (shootAction.IsPressed() && allowedToShoot)
        {
            StartCoroutine(Shoot());
        }
            
    }

    void FixedUpdate()
    {
        if (movementInputVector.magnitude > 0f)
        {
            Move();
        }
        else if (gamepadInputVector.magnitude > 0f)
        {
            GamepadMove();
        }
    }

    void Move()
    {
        // Moving back and forth
        Vector3 movement = transform.forward * movementInputVector.y * movementSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        // Rotating
        Vector3 rotationVector = new Vector3(0, movementInputVector.x * turnSpeed * Time.deltaTime * 100f, 0);
        Quaternion rotation = Quaternion.Euler(rotationVector);
        rb.MoveRotation(rb.rotation * rotation);
    }

    void GamepadMove()
    {
        // Reform the input vector to the right plane
        Vector3 movementVector = new Vector3(gamepadInputVector.x, 0, gamepadInputVector.y);

        /* Eplanation of isometric movement can be found here: https://youtu.be/8ZxVBCvJDWk */

        // Create a matrix
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

        // Skewer the input vector 45 degrees to accomodate for the isometric perspective
        Vector3 skewedVector = isoMatrix.MultiplyPoint3x4(movementVector);
        
        Vector3 movement = skewedVector * MovementSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);
        transform.LookAt(transform.position + skewedVector);
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
        
        GameObject spawnedBullet = Instantiate(
            original: bullet, 
            position: bulletSpawner.transform.position, 
            rotation: transform.rotation * spreadDirection
            );
    }

    Quaternion ComputeBulletSpread()
    {
        // Produce a random rotation within a certain radius
        Vector3 randomDirection = transform.forward + Random.insideUnitSphere * bulletSpread;

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
            movementSpeed = speedBeforeBoost * boostSpeedMultiplier;
        }
        else
        {
            // Reset movement speed
            movementSpeed = speedBeforeBoost;
        }
    }

    IEnumerator BoostCooldown()
    {
        allowedToBoost = false;
        yield return new WaitForSeconds(boostCooldownTime);
        allowedToBoost = true;
    }

    public float MovementSpeed 
    {
        get { return movementSpeed; } 
        
        // Any time movement speed is altered from another script, it updates the speedBeforeBoost value to reflect the new speed
        set { movementSpeed = value; speedBeforeBoost = movementSpeed; } 
    }
    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float BulletSpread { get { return bulletSpread; } set { bulletSpread = value; } }
    public float BulletRange { get { return bulletRange; } set { bulletRange = value; } }
    public float BulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }
    public float BoostSpeedMultiplier { get { return boostSpeedMultiplier; } set { boostSpeedMultiplier = value; } }
    public float BoostDuration { get { return boostDuration; } set { boostDuration = value; } }
    public float BoostCooldownTime { get { return boostCooldownTime; } set { boostCooldownTime = value; } }

}
