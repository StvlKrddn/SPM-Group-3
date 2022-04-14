using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{

    [Header("Stats: ")]
    [SerializeField] private float timeBetweenWave = 3f;
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float material = 0f;
    [SerializeField] private float money = 350f;
    [SerializeField] private float victoryWave = 9f;
    
    [Header("Enemies: ")]
    [SerializeField] private Transform regularEnemy;
    [SerializeField] private float timeBetweenEnemy = 0.5f;
    [SerializeField] private Transform spawnPosition;

    [Header("UI Elements: ")]
    [SerializeField] private Text waveUI;
    [SerializeField] private Text liveUI;
    [SerializeField] private Text moneyUI;
    [SerializeField] private Text materialUI;

    private int currentWave = 0;
    private bool waveOff = false;
    private float timer = 0;

    // Event that other scripts can subscribe to. Invoked when a new wave starts
    public event Action OnNewWave;

    private int[] enemiesAmount = { 3, 3, 5, 5, 8 };
    public int amountOfWaves;

    public Vector3[] theWaves;

    public float Money { get { return money; } set { money = value; } }

    private void Start()
    {
        UpdateResourcesUI();
    }

    void Update()
    {
        if (timer >= timeBetweenWave)
        {
            StartCoroutine(SpawnWave());
            timer = 0;
            waveOff = true;
        }

        if (waveOff == false)
        {
            timer += Time.deltaTime;
        }
    }

    private IEnumerator SpawnWave()
    {
        // Invoke event if not null
        OnNewWave?.Invoke();
        currentWave++;
        if (currentWave > victoryWave) //Victory condition
        {
            Victory();
            yield break;
        }

        //   for (int i = 0; i < theWaves[currentWave].Length; i++)
        //   {
        //       StartCoroutine(SpawnEnemies(regularEnemy, theWaves[currentWave][i]));
        //   }


        int waveLength = 0;
        switch (currentWave) //Indivdually controls each lane spawns and length
        {   
            case 1:
                waveLength = 5;
                yield return new WaitForSeconds(3);
                StartCoroutine(SpawnEnemies(regularEnemy, 6));
                break;

            case 2:
                waveLength = 5;
                StartCoroutine(SpawnEnemies(regularEnemy, 9));
                break;
            case 3:
                waveLength = 10;
                StartCoroutine(SpawnEnemies(regularEnemy, 12));
                break;
            case 4:
                waveLength = 10;
                StartCoroutine(SpawnEnemies(regularEnemy, 15));
                break;
            case 5:
                waveLength = 12;
                StartCoroutine(SpawnEnemies(regularEnemy, 15));
                break;
            case 6:
                waveLength = 14;
                StartCoroutine(SpawnEnemies(regularEnemy, 15));
                break;
            case 7:
                waveLength = 16;
                StartCoroutine(SpawnEnemies(regularEnemy, 18));
                break;
            case 8:
                waveLength = 18;
                StartCoroutine(SpawnEnemies(regularEnemy, 22));
                break;
            case 9:
                waveLength = 20;
                StartCoroutine(SpawnEnemies(regularEnemy, 26));
                break;


        }
        yield return new WaitForSeconds(waveLength);
        waveOff = false;
    }

    IEnumerator SpawnEnemies(Transform enemyType, float count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(enemyType, spawnPosition.position, spawnPosition.rotation); //Spawn enemy and wait for time between enemy
            yield return new WaitForSeconds(timeBetweenEnemy);
        }
        yield return false;
    }

    public void TakeDamage(float damage)
    {
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
            Defeat();
        }
        UpdateResourcesUI();
    }

    private void UpdateResourcesUI()
    {
        moneyUI.text = "Money: " + money;
        materialUI.text = "Material: " + material;
        waveUI.text = "Current Wave: " + currentWave;
        liveUI.text = "Lives: " + baseHealth;
    }

    public void AddMoney(float addMoney)
    {
        money += addMoney;
        UpdateResourcesUI();
    }

    public void AddMaterial(float addMaterial)
    {
        material += addMaterial;
        UpdateResourcesUI();
    }

    public bool spendResources(float moneySpent, float materialSpent)
    {
        if (moneySpent <= money && materialSpent <= material)
        {
            money -= moneySpent;
            material -= materialSpent;
            UpdateResourcesUI();
            return true;
        }
        //Show Error
        return false;
    }


    private void Victory()
    {
        Debug.Log("Victory");
    }

    private void Defeat()
    {
        Debug.Log("Defeat");
    }

}
