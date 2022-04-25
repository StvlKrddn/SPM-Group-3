using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WaveManager : MonoBehaviour
{
    [Header("Enemies: ")]
    [SerializeField] private bool spawnEnemies = true;
    [SerializeField] private List<GameObject> enemyTypes;
    [SerializeField] private float timeBetweenEnemy = 0.5f;
    [SerializeField] private Transform spawnPosition;

    [Header("Player")]
    [SerializeField] private PlayerMode startingMode;

    public WaveInfo[] waves;

    private int currentWave = 0;
    private int spawnRate = 0;
    public List<GameObject> currentWaveEnemies = new List<GameObject>();

    private void Start()
    {
        WaveConstructor(waves[currentWave]);
        //WaveConstructor(waves[1]);
    }

    void WaveConstructor(WaveInfo wave)
    {
        currentWaveEnemies.Clear();
        foreach (SubWave subWave in wave.subWaves)
        {
            List<GameObject> subWaveEnemies = new List<GameObject>();
            for (int i = 0; i < subWave.enemies.Length; i++)
            {
                for (int j = 0; j < subWave.enemies[i].amount; j++)
                {
                    subWaveEnemies.Add(subWave.enemies[i].enemyPrefab);
                }
            }
            Shuffle(subWaveEnemies);
            currentWaveEnemies.AddRange(subWaveEnemies);
        }
        spawnRate = wave.waveDuration / currentWaveEnemies.Count;
    }

    void Shuffle(List<GameObject> list)
    {
        int n = list.Count;
        while (n > 1)
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
        if (spawnEnemies)
        {
        //    if (timer >= timeBetweenWave)
            {
                //StartCoroutine(SpawnWave());
//                timer = 0;
  //              waveOff = true;
            }

    //        if (waveOff == false)
            {
      //          timer += Time.deltaTime;
            }
        }
    }

    IEnumerator SpawnCurrentWave()
    {
        for (int i = 0; i < currentWaveEnemies.Count; i++)
        {
            GameObject g = null; 
            g = Instantiate(currentWaveEnemies[i], spawnPosition.position, spawnPosition.rotation); //Spawn enemy and wait for time between enemy
            g.SetActive(true);
            yield return new WaitForSeconds(spawnRate);
        }
        yield return false;
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
