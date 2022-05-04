using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankState : MonoBehaviour
{
    [SerializeField] private float health = 50f;

    Transform spawnPoint;
    Transform garage;
    PlayerInput playerInput;

    float currentHealth;
    float playerID;

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

        // Subscribe to events
        EventHandler.Instance.RegisterListener<NewWaveEvent>(OnNewWave);
    }

    

    void InitializeInputSystem()
    {
        playerInput = transform.parent.GetComponent<PlayerInput>();

        playerID = playerInput.playerIndex;
    }

    void SetPlayerColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = playerInput.playerIndex == 0 ? Color.blue : Color.red;
    }

    void FindGarage()
    {
        garage = GameObject.Find("Garage").transform;
        spawnPoint = garage.Find("PlayerSpawn");
        transform.position = spawnPoint.position;
    }

    void OnDisable()
    {
        // Unsubscribe to events to avoid memory leaks
        //EventHandler.Instance.UnregisterListener<NewWaveEvent>(OnNewWave);
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
