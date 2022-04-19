using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class TankController : MonoBehaviour
{

    // Inspector variables
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float health = 50f;

    // Components
    protected Rigidbody rb;
    protected Transform turretObject;
    protected Transform spawnPoint;
    protected Transform garage;
    protected GameManager gameManager;

    // Input components
    protected InputAction moveGamepadAction;
    protected InputAction aimAction;
    protected PlayerInput playerInput;
    protected PlayerInputManager inputManager;

    // Instance variables
    protected Vector2 gamepadInputVector;
    protected Vector3 aimInputVector;
    protected float playerID;
    protected float aimSpeed;
    protected float currentHealth;
    protected float standardSpeed;
    protected Matrix4x4 isoMatrix;

    // Getters and Setters
    public float StandardSpeed { get { return standardSpeed; } set { standardSpeed = value; } }
    
    public PlayerInput PlayerInput { get { return playerInput; } }

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        InitializeInputSystem();

        SetPlayerColor();

        currentHealth = health;
        standardSpeed = movementSpeed;

        rb = GetComponent<Rigidbody>();

        turretObject = transform.GetChild(0);

        garage = GameObject.Find("Garage").transform;
        spawnPoint = garage.Find("PlayerSpawn");

        transform.position = spawnPoint.position;
        
        aimSpeed = standardSpeed * 5;

        //Create isometric matrix
        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        /* Eplanation of isometric translation can be found here: https://youtu.be/8ZxVBCvJDWk */

        // Subscribe to events
        EventHandler.Instance.RegisterListener<NewWaveEvent>(OnNewWave);
    }

    void OnDestroy()
    {
        // Unsubscribe to events to avoid memory leaks
        EventHandler.Instance.UnregisterListener<NewWaveEvent>(OnNewWave);
    }
    
    void InitializeInputSystem()
    {
        playerInput = GetComponent<PlayerInput>();
        inputManager = gameManager.GetComponent<PlayerInputManager>();

        playerID = playerInput.playerIndex;

        moveGamepadAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
    }

    public void SetPlayerColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (playerID == 0)
        {
            renderer.material.color = Color.blue;
        }
        else
        {
            renderer.material.color = Color.red;
        }
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
        // Skewer the input vector 45 degrees to accomodate for the isometric perspective
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

    void TakeDamage(float damage)
    {
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
        print("New Wave");
        currentHealth = health;
        if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Parked"))
        {
            print("Parked!");
            playerInput.SwitchCurrentActionMap("Tank");
        }
    }
}