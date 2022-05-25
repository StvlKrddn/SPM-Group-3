using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health))]
public class TankState : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float health = 50f;
    [SerializeField] private Animator animator;
    public int levelOfTank;



    // Components
    Rigidbody rb;
    Transform turretObject;

    // Input components
    InputAction moveAction;
    InputAction aimAction;
    InputAction abilityAction;

    // Instance variables
    Vector2 gamepadInputVector;
    protected Vector3 aimInputVector;
    float aimSpeed;
    float standardSpeed;
    Matrix4x4 isoMatrix;

    Transform spawnPoint;
    Transform garage;
    PlayerInput playerInput;
    PlayerHandler playerHandler;

    float currentHealth;
    float playerID;
    public TankUpgradeTree tankUpgradeTree;
    [SerializeField] private TankUpgradeTree tankUpgradeTreeOne;
    [SerializeField] private TankUpgradeTree tankUpgradeTreeTwo;
    [SerializeField] private UnityEngine.Material player1Material;
    [SerializeField] private UnityEngine.Material player2Material;

    private int hurMangaGangerDamage = 0; 


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

        turretObject = transform.GetChild(0);
        
        aimSpeed = standardSpeed * 5;

        StartCoroutine(LockRotation());

        //Create isometric matrix
        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        /* Explanation of isometric translation can be found here: https://youtu.be/8ZxVBCvJDWk */

        // Subscribe to events
        EventHandler.Instance.RegisterListener<WaveEndEvent>(OnWaveEnd);

    }

    IEnumerator LockRotation()
    {
        yield return new WaitForSeconds(0.5f);
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
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
        Renderer renderer = GetComponent<Renderer>();
        if (playerInput.playerIndex == 0)
        {
            renderer.material = player1Material;
            transform.Find("TankBody").Find("Cube.004").GetComponent<Renderer>().material = player1Material;
        }
        else
        {
            renderer.material = player2Material;
            transform.Find("TankBody").Find("Cube.004").GetComponent<Renderer>().material = player2Material;
        }
        tankUpgradeTree = playerInput.playerIndex == 0 ? tankUpgradeTreeOne : tankUpgradeTreeTwo;
    }

    void FindGarage()
    {
        garage = GameObject.FindGameObjectWithTag("Garage").transform;
        spawnPoint = garage.Find("PlayerSpawn");
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
    }

    void FixedUpdate()
    {
        animator.SetBool("isMoving", moveAction.IsPressed());

        Move();
        //levelOfTank = UpgradeController.instance.currentUpgradeLevel;
    }

    void Move()
    {
        // Translate the input vector to the right plane
        Vector3 movementVector = new Vector3(gamepadInputVector.x, 0, gamepadInputVector.y);

        // Translate vector to an isometric viewpoint
        Vector3 skewedVector = TranslateToIsometric(movementVector);
        
        Vector3 movement = skewedVector * standardSpeed * Time.deltaTime;

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
        if (other.CompareTag("EnemyBullet"))
        {
            EnemyBullet enemyBullet = other.gameObject.GetComponent<EnemyBullet>();
            TakeDamage(enemyBullet.damage);
        }
        else if (other.CompareTag("MortarBullet"))
        {
            hurMangaGangerDamage += 1; 
            EnemyMortarShot enemyMortarShot = other.gameObject.GetComponentInParent<EnemyMortarShot>();
            TakeDamage(enemyMortarShot.damage);

            print("Tar man damage flera gångar?" + hurMangaGangerDamage);
        }
    }

	private void OnCollisionEnter(Collision collision)
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
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            DestroyTank();
        }
    }

    void DestroyTank()
    {
        print("Tank destroyed!");
        transform.position = spawnPoint.position;
        playerHandler.Destroyed = true;
            
        EventHandler.Instance.InvokeEvent(new PlayerSwitchEvent(
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
        currentHealth = health;
    }

    public void IncreaseSpeed(float speedIncrease)
    {
        standardSpeed += speedIncrease;
        GetComponent<BoostAbility>().ChangeSpeed();
    }
}
