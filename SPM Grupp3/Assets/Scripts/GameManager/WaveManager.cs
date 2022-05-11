using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WaveManager : MonoBehaviour
{
    [Header("Enemies: ")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Text waveUI;

    public WaveInfo[] waves;

    [Header("Wave Clear UI: ")]
    [SerializeField] private GameObject waveClear;
    private int enemyCount;
    private int kills;

    private int currentWave;
    private int victoryWave;
    private int spawnRate = 0;
    private List<GameObject> currentWaveEnemies = new List<GameObject>();

    private void Awake()
    {
        victoryWave = waves.Length;
        waveClear.SetActive(false);
    }

    private void Start()
    {
      //  StartWave(0);
    }

    public void StartWave(int currentWave)
    {
        waveClear.SetActive(false);
        this.currentWave = currentWave;
        WaveConstructor(waves[currentWave]);
        StartCoroutine(SpawnCurrentWave());
        UpdateUI();
 //       Debug.Log(enemyCount);
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
        enemyCount = currentWaveEnemies.Count;
        spawnRate = wave.waveDuration / currentWaveEnemies.Count;
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

    void Update()
    {
    }

    public void WaveUpdate()
    {
        enemyCount--;
        GameManager.Instance.EnemiesKilled++;
        if (enemyCount == 0)
        {
            currentWave++;
            
            if (currentWave >= victoryWave)
            {
                GameManager.Instance.Victory(kills);
            }
            else
            {
                waveClear.SetActive(true);

                FindObjectOfType<GameManager>().spawnEnemies = true;
                Debug.Log("Wave " + currentWave + " cleared");

                EventHandler.Instance.InvokeEvent(new WaveEndEvent(
                    description: "wave ended"
                ));

                //Activates the the button so the players can start next round 
            }
        }
    }

    private IEnumerator SpawnCurrentWave()
    {
        for (int i = 0; i < currentWaveEnemies.Count; i++)
        {
            GameObject g = Instantiate(currentWaveEnemies[i], spawnPosition.position, spawnPosition.rotation); //Spawn enemy and wait for time between enemy
            g.SetActive(true);
            yield return new WaitForSeconds(spawnRate);
        }
        yield return false;
    }

    private void UpdateUI()
    {
        waveUI.text = (currentWave + 1) + "/" + victoryWave;
    }
}

[Serializable]
public struct WaveInfo
{
    public int waveDuration;
    public SubWave[] subWaves;
}

[Serializable]
public struct SubWave
{
    public EnemyStruct[] enemies;
}

[Serializable]
public struct EnemyStruct
{
    public GameObject enemyPrefab;
    public int amount;
}
