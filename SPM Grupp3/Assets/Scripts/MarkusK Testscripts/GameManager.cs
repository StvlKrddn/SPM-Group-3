using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Stats: ")]
    [SerializeField] private float timeBetweenWave = 3f;
    [SerializeField] private float baseHealth = 100f; // Change basestats of our game
    [SerializeField] private float material = 0f;
    [SerializeField] private float money = 350f;
    [SerializeField] private float victoryWave = 5f;
    
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

    public event Action OnNewWave;

    private int[] enemiesAmount = { 3, 3, 5, 5, 8 };
    public int amountOfWaves;

    public Vector3[] theWaves;

    public float Money { get { return money; } set { money = value; } }

    private void Start()
    {
        UpdateResourcesUI();
    }

    // Update is called once per frame
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
                StartCoroutine(SpawnEnemies(regularEnemy, 3));
                break;

            case 2:
                waveLength = 5;
                StartCoroutine(SpawnEnemies(regularEnemy, 3));
                break;
            case 3:
                waveLength = 10;
                StartCoroutine(SpawnEnemies(regularEnemy, 5));
                break;
            case 4:
                waveLength = 10;
                StartCoroutine(SpawnEnemies(regularEnemy, 5));
                break;
            case 5:
                waveLength = 12;
                StartCoroutine(SpawnEnemies(regularEnemy, 8));
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
