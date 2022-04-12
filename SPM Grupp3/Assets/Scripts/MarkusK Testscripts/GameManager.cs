using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int baseHealth = 100; // Change basestats of our game
    public int material = 0;
    public int money = 350;
    public int currentWave = 0;    
    public int victoryWave = 10;

    public Transform regularEnemy;
    public Transform spawnPosition;

    public Text waveUI;
    public Text liveUI;
    public Text moneyUI;
    public Text materialUI;

    public bool waveOff = false;
    public float timer = 0;
    public float timeBetweenWave = 3f;
    public float timeBetweenEnemy = 0.5f;

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
            UpdateResourcesUI();
            timer = 0;
            waveOff = true;
        }

        if (waveOff == false)
        {
            timer += Time.deltaTime;
        }
    }

    private IEnumerator SpawnWave ()
    {
        currentWave++;
        if (currentWave > victoryWave) //Victory condition
        {
            Victory();
            yield break;
        }

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
        }
        yield return new WaitForSeconds(waveLength);
        waveOff = false;
    }

    IEnumerator SpawnEnemies(Transform enemyType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(enemyType, spawnPosition.position, spawnPosition.rotation); //Spawn enemy and wait for time between enemy
            yield return new WaitForSeconds(timeBetweenEnemy);
        }
        yield return false;
    }

    public void TakeDamage(int damage)
    {
        baseHealth -= damage;
        UpdateResourcesUI();
        if (baseHealth <= 0)
        {
            Defeat();
        }
    }

    public void AddMoney(int addMoney)
    {
        money += addMoney;
        UpdateResourcesUI();
    }

    public void AddMaterial(int addMaterial)
    {
        money += addMaterial;
        UpdateResourcesUI();
    }

    public bool spendResources(int moneySpent, int materialSpent)
    {
        if (moneySpent < money && materialSpent < material)
        {
            money -= moneySpent;
            material -= materialSpent;
            UpdateResourcesUI();
            return true;
        }
        //Show Error
        return false;
    }

    private void UpdateResourcesUI()
    {
        moneyUI.text = "Money: " + money;
        materialUI.text = "Material: " + material;
        waveUI.text = "Current Wave: " + currentWave;
        liveUI.text = "Lives: " + baseHealth; 
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
