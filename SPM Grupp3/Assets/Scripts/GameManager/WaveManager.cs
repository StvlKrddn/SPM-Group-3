using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WaveManager : MonoBehaviour
{
    [Header("Enemies: ")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject enemyContainer;
    
    [Space]
    public WaveInfo[] waves;

    [Header("Debug")]
    [SerializeField] private int startingWave = 1;

    private int enemyCount;
    private GameManager gameManager;
    private int currentWave = -1;
    private int victoryWave;
    private float spawnRate = 0;
    private bool spawnEnemies = true;
    private int waveMoneyBonus;
    private List<GameObject> currentWaveEnemies = new List<GameObject>();
    private Text waveUI;
    private GameObject waveClear;

    private void Awake()
    {
        if (startingWave != 1)
        {
            currentWave = startingWave - 2;
        }

        Transform waveHolder = UI.Canvas.transform.GetChild(0).Find("WaveHolder");
        waveUI = waveHolder.Find("WaveCounter").GetComponent<Text>();
        waveClear = waveHolder.Find("WaveCleared").gameObject;
        
        victoryWave = waves.Length;
        waveClear.SetActive(false);
        gameManager = GameManager.Instance;


        EventHandler.Instance.RegisterListener<StartWaveEvent>(OnStartWave);
    }


    private void OnStartWave(StartWaveEvent eventInfo)
    {
        if (spawnEnemies)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        currentWave++;

        EventHandler.Instance.InvokeEvent(new NewWaveEvent(
            description: "New wave started",
            currentWave: currentWave
            ));

        spawnEnemies = false;
    }

    public void StartWave(int currentWave)
    {
        waveClear.SetActive(false);
        this.currentWave = currentWave;
        WaveConstructor(waves[currentWave]);
        StartCoroutine(SpawnCurrentWave());
        UpdateUI();
    }

    private void WaveConstructor(WaveInfo wave)
    {
        currentWaveEnemies.Clear();
        foreach (SubWave subWave in wave.subWaves)
        {
            List<GameObject> subWaveEnemies = new List<GameObject>(); //Goes through each subwave and shuffles it
            for (int i = 0; i < subWave.enemies.Length; i++)
            {
                for (int j = 0; j < subWave.enemies[i].amount; j++)
                {
                    subWaveEnemies.Add(subWave.enemies[i].enemyPrefab);
                }
            }
            Shuffle(subWaveEnemies);
            currentWaveEnemies.AddRange(subWaveEnemies); //Then adds the shuffled subwave to the wave
        }
        waveMoneyBonus = wave.waveMoneyBonus;
        enemyCount = currentWaveEnemies.Count;
    }

    void Shuffle(List<GameObject> list)
    {
        int n = list.Count;
        while (n > 1) //Randomizes the list
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void WaveUpdate()
    {
        enemyCount--;
        gameManager.EnemiesKilled++;
        if (enemyCount == 0)
        {
            if (currentWave >= victoryWave - 1)
            {
                gameManager.Victory();
            }
            else
            {
                waveClear.SetActive(true);

                spawnEnemies = true;
                Debug.Log("Wave " + currentWave + " cleared");
                GameManager.Instance.AddMoney(waveMoneyBonus);

                EventHandler.Instance.InvokeEvent(new WaveEndEvent(
                    description: "wave ended",
                    currentWave: currentWave
                ));

                //Activates the the button so the players can start next round 
            }
        }
    }

    private IEnumerator SpawnCurrentWave()
    {
        for (int i = 0; i < currentWaveEnemies.Count; i++)
        {
            GameObject g = Instantiate(currentWaveEnemies[i], spawnPosition.position, currentWaveEnemies[i].transform.rotation, enemyContainer.transform); //Spawn enemy and wait for time between enemy
            g.SetActive(true);
            yield return new WaitForSeconds(spawnRate);
        }
        yield return false;
    }

    [ContextMenu("Calculate Wave Duration")]
    public void CalculateWaveDuration()
    {
		for (int i = 0; i < waves.Length; i++)
        {
			WaveInfo waveInfo = waves[i];
			waveInfo.waveDuration = 0;

			for (int j = 0; j < waveInfo.subWaves.Length; j++)
            {
				SubWave subwave = waveInfo.subWaves[j];
				for (int k = 0; k < subwave.enemies.Length; k++)
                {
					EnemyStruct enemy = subwave.enemies[k];
                    waveInfo.waveDuration += enemy.amount * subwave.spawnRate;
                }
            }
            print("Wave " + (i + 1) + " is " + waveInfo.waveDuration + " seconds long");
        }
    }

    public void UpdateUI()
    {
        waveUI.text = (currentWave + 1) + "/" + victoryWave;
    }

    public void Restart()
    {
        currentWave = -1;
        foreach (Transform enemy in enemyContainer.transform)
        {
            Destroy(enemy.gameObject);
        }
        StopCoroutine(SpawnCurrentWave());
        currentWaveEnemies.Clear();
        UpdateUI();
        spawnEnemies = true;
    }
}

[Serializable]
public struct WaveInfo
{
    public string waveName;
    public float waveDuration;
    public int waveMoneyBonus;
    public SubWave[] subWaves;
}

[Serializable]
public struct SubWave
{
    public EnemyStruct[] enemies;
    public float spawnRate;
 
}

[Serializable]
public struct EnemyStruct
{
    public GameObject enemyPrefab;
    public int amount;
}
