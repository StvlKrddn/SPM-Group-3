using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankState : MonoBehaviour
{
    // Inspector variables
    public float movementSpeed = 6f;

    // Components
    Rigidbody rb;
    Transform turretObject;

    // Input components
    InputAction moveGamepadAction;
    InputAction aimAction;
    InputAction abilityAction;

    // Instance variables
    Vector2 gamepadInputVector;
    protected Vector3 aimInputVector;
    float aimSpeed;
    float standardSpeed;
    Matrix4x4 isoMatrix;

    [SerializeField] private float health = 50f;

    Transform spawnPoint;
    Transform garage;
    PlayerInput playerInput;

    float currentHealth;
    float playerID;
    public static TankUpgradeTree tankUpgradeTreeOne;
    public static TankUpgradeTree tankUpgradeTreeTwo;


    // Getters and Setters
    public float StandardSpeed { 
        get 
        { 
            // Shady code until i figure out a fix
            if (standardSpeed == 0)
            {
                standardSpeed = movementSpeed;
            }
            return standardSpeed; 
        } 
        set { standardSpeed = value; } 
    }

    public PlayerInput PlayerInput { 
        get 
        {
            if (playerInput == null)
            {
                playerInput = GetComponent<PlayerInput>();
            }
            return playerInput; 
        } 
    }

    void Awake()
    {
        InitializeInputSystem();

        SetPlayerColor();

        FindGarage();

        currentHealth = health;

        standardSpeed = movementSpeed;

        rb = GetComponent<Rigidbody>();

        turretObject = transform.GetChild(0);
        
        aimSpeed = standardSpeed * 5;

        if (GetComponent<TankUpgradeTree>())
        {
            tankUpgradeTreeOne = GetComponent<TankUpgradeTree>();
        }

        //Create isometric matrix
        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        /* Explanation of isometric translation can be found here: https://youtu.be/8ZxVBCvJDWk */

        // Subscribe to events
        EventHandler.Instance.RegisterListener<NewWaveEvent>(OnNewWave);
    }

    void InitializeInputSystem()
    {
        playerInput = transform.parent.GetComponent<PlayerInput>();

        playerID = playerInput.playerIndex;

        moveGamepadAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
        abilityAction = playerInput.actions["Ability"];
        abilityAction.performed += AbilityCast;
    }

    void SetPlayerColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = playerInput.playerIndex == 0 ? Color.blue : Color.red;
    }

    void FindGarage()
    {
        garage = GameObject.FindGameObjectWithTag("Garage").transform;
        spawnPoint = garage.Find("PlayerSpawn");
        transform.position = spawnPoint.position;
    }
    void Update()
    {
        gamepadInputVector = moveGamepadAction.ReadValue<Vector2>();
        aimInputVector = aimAction.ReadValue<Vector2>();

        RotateTurret(); 
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Translate the input vector to the right plane
        Vector3 movementVector = new Vector3(gamepadInputVector.x, 0, gamepadInputVector.y);

        // Translate vector to an isometric viewpoint
        Vector3 skewedVector = TranslateToIsometric(movementVector);
        
        Vector3 movement = skewedVector * standardSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);

        if (moveGamepadAction.IsPressed())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(skewedVector), Time.deltaTime * standardSpeed);
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

    Vector3 TranslateToIsometric(Vector3 vector)
    {
        // Skewer the input vector 45 degrees to accommodate for the isometric perspective
        return isoMatrix.MultiplyPoint3x4(vector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            EnemyBullet enemyBullet = other.gameObject.GetComponent<EnemyBullet>();
            TakeDamage(enemyBullet.damage);
        }
    }

    private void AbilityCast(InputAction.CallbackContext context)
    {
        if (tankUpgradeTreeOne != null)
        {
            tankUpgradeTreeOne.Ability();
        }
    }

    void TakeDamage(float damage)
    {
        print("Taking damage");
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            print("Tank destroyed!");
            DestroyTank();
        }
    }

    void DestroyTank()
    {
        transform.position = spawnPoint.position;
    }

    void OnNewWave(NewWaveEvent eventInfo)
    {
        currentHealth = health;
    }
}
