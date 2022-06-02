using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankState : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float health = 50f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private ParticleSystem[] hitEffects;
    public int LevelOfTank;

    // Components
    private Rigidbody rb;

    // Input components
    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction abilityAction;

    // Instance variables
    private Vector2 gamepadInputVector;
    private float aimSpeed;
    private float standardSpeed;
    private Matrix4x4 isoMatrix;
    private Transform garage;
    private PlayerInput playerInput;
    private PlayerHandler playerHandler;
    private float currentHealth;
    private Transform spawnPoint;
    private float playerID;
    private int hurMangaGangerDamage = 0;
    private bool invincibilityFrame = false;
    private Color player1Color;
    private Color player2Color;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TankUpgradeTree tankUpgradeTreeOne;
    [SerializeField] private TankUpgradeTree tankUpgradeTreeTwo;

    protected Vector3 aimInputVector;

    public TankUpgradeTree tankUpgradeTree;
    public Transform TurretObject;

    // Getters and Setters
    public float StandardSpeed { 
        get 
        { 
            if (standardSpeed == 0)
            {
                standardSpeed = movementSpeed;
            }
            return standardSpeed; 
        } 
        set { standardSpeed = value; } 
    }
    public float Health { get { return health; } }

    public PlayerInput PlayerInput { 
        get 
        {
            if (playerInput == null)
            {
                playerInput = GetComponentInParent<PlayerInput>();
            }
            return playerInput; 
        } 
    }

    void Awake()
    {
        InitializeInputSystem();

        SetPlayerDifference();

        FindGarage();

        currentHealth = health;

        standardSpeed = movementSpeed;

        rb = GetComponent<Rigidbody>();

        playerHandler = GetComponentInParent<PlayerHandler>();

        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.slider.maxValue = health;
        healthBar.HandleHealthChanged(health);

        TurretObject = transform.GetChild(0);
        
        aimSpeed = standardSpeed * 5;


        //Create isometric matrix
        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        /* Explanation of isometric translation can be found here: https://youtu.be/8ZxVBCvJDWk */

        // Subscribe to events
        EventHandler.RegisterListener<WaveEndEvent>(OnWaveEnd);
    }

    private void OnEnable()
    {
        healthBar.HandleHealthChanged(currentHealth);
        invincibilityFrame = false;
    }

    void InitializeInputSystem()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        playerID = playerInput.playerIndex;

        moveAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
        abilityAction = playerInput.actions["Ability"];
    }

    void SetPlayerDifference()
    {
        player1Color = GameManager.Instance.Player1Color;
        player2Color = GameManager.Instance.Player2Color;
        if (playerInput.playerIndex == 0)
        {
            transform.Find("TankMesh").Find("TankBody").GetComponent<Renderer>().material.color = player1Color;
        }
        else
        {
            transform.Find("TankMesh").Find("TankBody").GetComponent<Renderer>().material.color = player2Color;
        }
        tankUpgradeTree = playerInput.playerIndex == 0 ? tankUpgradeTreeOne : tankUpgradeTreeTwo;
        if (playerInput.playerIndex == 0)
        {
            tankUpgradeTreeTwo.enabled = false;
        }
        else
        {
            tankUpgradeTreeOne.enabled = false;
        }
    }

    void FindGarage()
    {
        garage = GameObject.FindGameObjectWithTag("Garage").transform;
        spawnPoint = garage.Find("SpawnPoints").GetChild(playerInput.playerIndex);
        transform.position = spawnPoint.position;
    }
    void Update()
    {
        gamepadInputVector = moveAction.ReadValue<Vector2>();
        aimInputVector = aimAction.ReadValue<Vector2>();

        RotateTurret();
        if (abilityAction.triggered)
        {
            Ability();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            DestroyTank();
        }
    }

    void FixedUpdate()
    {
        animator.SetBool("isMoving", moveAction.IsPressed());

        Move();
    }

    void Move()
    {
        // Translate the input vector to the right plane
        Vector3 movementVector = new Vector3(gamepadInputVector.x, 0, gamepadInputVector.y);

        // Translate vector to an isometric viewpoint
        Vector3 skewedVector = TranslateToIsometric(movementVector);
        
        Vector3 movement = standardSpeed * Time.deltaTime * skewedVector;

        rb.MovePosition(transform.position + movement);

        if (moveAction.IsPressed())
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
            TurretObject.rotation = Quaternion.Slerp(TurretObject.rotation, Quaternion.LookRotation(skewedVector), Time.deltaTime * aimSpeed);
        }
    } 

    Vector3 TranslateToIsometric(Vector3 vector)
    {
        // Skewer the input vector 45 degrees to accommodate for the isometric perspective
        return isoMatrix.MultiplyPoint3x4(vector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            EnemyBullet enemyBullet = other.gameObject.GetComponent<EnemyBullet>();
            TakeDamage(enemyBullet.Damage);
        }
        else if (other.CompareTag("MortarBullet"))
        {
            hurMangaGangerDamage += 1; 
            EnemyMortarShot enemyMortarShot = other.gameObject.GetComponentInParent<EnemyMortarShot>();
            TakeDamage(enemyMortarShot.Damage);

        }
    }

	private void OnCollisionStay(Collision collision)
	{
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            TakeDamage(enemy.MeleeDamage);
        }
	}

    private void Ability()
    {
        if (tankUpgradeTree != null)
        {
            tankUpgradeTree.Ability();
        }
    }

    public void TakeDamage(float damage)
    {

        if (!invincibilityFrame)
        {
            Invoke(nameof(InvincibilityDuration), 0.15f);
            invincibilityFrame = true;
            currentHealth -= damage;
            healthBar.HandleHealthChanged(currentHealth);

            for(int i = 0; i < hitEffects.Length; i++)
                hitEffects[i].Play();

            if (currentHealth <= 0 && !playerHandler.Destroyed)
            {
                DestroyTank();
            }
        }
    }

    public void InvincibilityDuration()
    {
        invincibilityFrame = false;
    }

    void DestroyTank()
    {
        print("Tank destroyed!");
        Instantiate(destroyEffect, transform.position, Quaternion.identity);

        transform.position = spawnPoint.position;
        playerHandler.Destroyed = true;
            
        EventHandler.InvokeEvent(new PlayerSwitchEvent(
            description: "Player switching mode",
            playerContainer: transform.parent.gameObject
        ));
    }

    void OnWaveEnd(WaveEndEvent eventInfo)
    {
        RepairTank();
    }

    public void RepairTank()
    {
        playerHandler.Destroyed = false;
        ResetHealth();
        //currentHealth = health;

        EventHandler.InvokeEvent(new EnterTankModeEvent(
            description: "Player switching mode",
            playerContainer: transform.parent.gameObject
        ));
    }

    public void IncreaseSpeed(float speedIncrease)
    {
        standardSpeed += speedIncrease;
        GetComponent<BoostAbility>().ChangeSpeed();
    }

    private void ResetHealth()
    {
        currentHealth = health;
        healthBar.HandleHealthChanged(health);
    }
}
