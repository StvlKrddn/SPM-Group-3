using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WaveManager : MonoBehaviour
{
    [Header("Enemies: ")]
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private int poolCount = 20;
    
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
    private List<GameObject> poolOfEnemies = new List<GameObject>();
    private Text waveUI;
    private GameObject startHint;
    private GameObject waveClear;
    private Dictionary<int, float> changeSpawnRate = new Dictionary<int, float>();
    private Waypoints wayPoints;
    private List<Transform[]> wayPostions;

    //public int timeToWaitBetweenWave; 

    private void Awake()
    {
        EventHandler.RegisterListener<NewWaveEvent>(OnStartWave);
        if (startingWave != 1)
        {
            currentWave = startingWave - 2;
        }
        Transform waveHolder = UI.Canvas.transform.GetChild(0).Find("WaveHolder");
        waveUI = waveHolder.Find("WaveCounter").GetComponent<Text>();
        waveClear = waveHolder.Find("WaveCleared").gameObject;
        startHint = waveHolder.Find("StartWaveHint").gameObject;
        
        victoryWave = waves.Length;
        waveClear.SetActive(false);
        gameManager = GameManager.Instance;
        currentWave = gameManager.CurrentWave;

    }

    private void OnEnable()
    {
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterListener<NewWaveEvent>(OnStartWave);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            currentWave += 1;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            currentWave -= 1;
        }

    }
    private void Start()
    {
        wayPoints = Waypoints.instance;
        wayPostions = wayPoints.GetWaypoints();
    }

    private void OnStartWave(NewWaveEvent eventInfo)
    {       
        if (spawnEnemies)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        currentWave++;

        waveClear.SetActive(false);
        startHint.SetActive(false);
        WaveConstructor(waves[currentWave]);
        StartCoroutine(SpawnCurrentWave());
        UpdateUI();

        spawnEnemies = false;
    }

    private void WaveConstructor(WaveInfo wave)
    {
        currentWaveEnemies.Clear();
        changeSpawnRate.Clear();

        spawnRate = wave.subWaves[0].spawnRate;
        foreach (SubWave subWave in wave.subWaves)
        {
            changeSpawnRate.Add(currentWaveEnemies.Count - 1, subWave.spawnRate); //Adds in each subwaves spawnrate into the list
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

    private void ClearInactive()
    {
        for (int i = poolOfEnemies.Count - 1; i > -1; i--)
        {
            if (poolOfEnemies.Count > poolCount && poolOfEnemies[i].activeSelf == false)
            {
                Destroy(poolOfEnemies[i]);
                poolOfEnemies.RemoveAt(i);
            }
        }
    }

    private int FindEmptyPool()
    {
        for (int i = 0; i < poolCount; i++)
        {
            if (poolOfEnemies.Count > i && poolOfEnemies[i] == null)
            {
                return i;
            }
        }
        return -1;
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
                if (poolOfEnemies.Count > poolCount) 
                {
                    ClearInactive();
                }
                waveClear.SetActive(true);
                //startHint.transform.position = waveClear.transform.position;
                startHint.transform.localPosition = new Vector3(startHint.transform.localPosition.x, -370, startHint.transform.localPosition.z);
                startHint.SetActive(true);
                spawnEnemies = true;
                Debug.Log("Wave " + currentWave + " cleared");
                GameManager.Instance.AddMoney(waveMoneyBonus);

                EventHandler.InvokeEvent(new WaveEndEvent(
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
            if (poolOfEnemies.Count > poolCount) //Clear inactive enemies
            {
                ClearInactive();
            }
            GameObject enemy = GetPooledEnemy(currentWaveEnemies[i]);
            int givenPath = wayPoints.GiveNewPath(); //Gives the enemy the right path
            if (enemy != null)
            {
                UseInactive(enemy, givenPath);
            }
            else
            {
                AddEnemy(currentWaveEnemies[i], givenPath);
            }
            //Spawn enemy and wait for time between enemy


            yield return new WaitForSeconds(spawnRate);

            if (changeSpawnRate.ContainsKey(i)) //The wave changes spawnrate after a subwave
            {
                spawnRate = changeSpawnRate[i];





                
            }
        }
        yield return false;
    }

    private void UseInactive(GameObject enemy, int givenPath)
    {
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.path = givenPath;
        enemyController.transform.position = wayPostions[enemyController.path][0].position;
        enemyController.gameObject.SetActive(true);
    }

    private void AddEnemy(GameObject enemy, int givenPath)
    {
        GameObject g = Instantiate(enemy, wayPostions[givenPath][0].position, enemy.transform.rotation, enemyContainer.transform);
        int index = FindEmptyPool();

        if (index > -1)
        {
            poolOfEnemies[index] = g;
        }
        else
        {
            poolOfEnemies.Add(g);
        }
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

    public GameObject GetPooledEnemy(GameObject enemy)
    {
        for (int i = 0; i < poolOfEnemies.Count; i++)
        {
            if (poolOfEnemies[i] != null && poolOfEnemies[i].activeSelf == false)
            {
                if (poolOfEnemies[i].GetComponent<EnemyController>().GetType() == enemy.GetComponent<EnemyController>().GetType())
                {
                    return poolOfEnemies[i];
                }
            }
        }
        return null;
    }

    [ContextMenu("Calculate Total money")]
    public void CalculateMoney()
    {

        int totalMoney = 0; 

        for (int i = 0; i < waves.Length; i++)
        {
            WaveInfo waveInfo = waves[i];

            totalMoney += waveInfo.waveMoneyBonus; 
        }

        print(totalMoney);
    }


    public void UpdateUI()
    {
        waveUI.text = (currentWave + 1) + "/" + victoryWave;
    }

    public void Restart()
    {
        currentWave = -1;
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
 //   public bool waitBetweenWave;
  //  public int timeToWait;
    //public bool waitBetweenWave; 


 
}

[Serializable]
public struct EnemyStruct
{
    public GameObject enemyPrefab;
    public int amount;
}
