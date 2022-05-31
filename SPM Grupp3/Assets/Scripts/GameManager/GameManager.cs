using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BaseStats baseStats;

    [Header("UI: ")]
    [SerializeField] private Text moneyCounterUI;
    [SerializeField] private Text materialCounterUI;
    [SerializeField] private ParticleSystem moneyParticle;
    [SerializeField] private ParticleSystem materialParticle;
    private Color moneyBaseColor;
    private Color materialBaseColor;
    [SerializeField] private GameObject SpendEffektPrefab;

    [NonSerialized] public Color Player1Color;
    [NonSerialized] public Color Player2Color;

    [Header("Other")]
    [NonSerialized] public List<PlacedTower> towersPlaced = new List<PlacedTower>();
    
    private GameObject victoryPanel;
    private GameObject defeatPanel;
    private GameObject waveCounter;

    private BuildManager buildManager;
    private GameObject damagingEnemy;
    private WaveManager waveManager;
    private Canvas canvas;
    private GameObject livesSlider;
    private HealthBar healthBar;
    private float money;
    private float material;
    private float baseHealth;
    private PlayerMode startingMode;

    private int currentWave = -1;
    private float currentBaseHealth;
    private int enemiesKilled;
    private int moneyCollected;
    private int materialCollected;

    public int CurrentWave { get { return currentWave; } set { currentWave = value; } }
    public PlayerMode StartingMode { get { return startingMode; } }
    public int EnemiesKilled { get { return enemiesKilled; } set { enemiesKilled = value; } }
    public float Money { get { return money; } set { money = value; } }
    public float Material { get { return material; } set { material = value; } }

    private static GameManager instance;
    public static GameManager Instance 
    { 
        get 
        {
            // "Lazy loading" to prevent Unity load order error
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance; 
        } 
    }

    void Awake() 
    {


        EventHandler.RegisterListener<SaveGameEvent>(SaveGame);
        buildManager = FindObjectOfType<BuildManager>();
        if (DataManager.FileExists(DataManager.SaveData))
        {
            LoadSaveData();
        }
        else 
        {
            LoadBase();
        }
        if (DataManager.FileExists(DataManager.CustomizationData))
        {
            LoadCustomizationData();
        }
    }

    private void LoadSaveData()
    {
        SaveData saveData = (SaveData) DataManager.ReadFromFile(DataManager.SaveData);
        money = saveData.money;
        material = saveData.material;
        currentWave = saveData.currentWave;
        baseHealth = saveData.currentBaseHealth;

        enemiesKilled = saveData.enemiesKilled;
        moneyCollected = saveData.moneyCollected;
        materialCollected = saveData.materialCollected;

        startingMode = saveData.startingMode;

        List<TowerData> towerData = new List<TowerData>(saveData.towerData);

        foreach (TowerData tower in towerData)
        {
            buildManager.LoadTower(tower);
        }
        UpgradeController.currentUpgradeLevel = saveData.tankUpgradeLevel;
        
        Invoke(nameof(FixUpgradeDelay), 0.01f);
    }

    private void LoadCustomizationData()
    {
        CustomizationData customData = (CustomizationData) DataManager.ReadFromFile(DataManager.CustomizationData);

        Player1Color = customData.player1Color;
        Player2Color = customData.player2Color;
    }

    private void FixUpgradeDelay(GameObject tank)
    {
        UpgradeController.Instance.FixUpgrades(FindObjectOfType<TankState>().gameObject);
        UpgradeController.Instance.FixUpgrades(tank);
    }

    private void LoadBase()
    {
        money = baseStats.money;
        material = baseStats.material;
        currentWave = -1;
        baseHealth = baseStats.baseHealth;
        startingMode = baseStats.startingMode;
        
        enemiesKilled = 0;
        moneyCollected = 0;
        materialCollected = 0;

        Player1Color = baseStats.Player1Color;
        Player2Color = baseStats.Player2Color;
    }

    [ContextMenu("Delete Saved Data")]
    public void DeleteSaveData()
    {
        DataManager.DeleteFile(DataManager.SaveData);
        DataManager.DeleteFile(DataManager.CustomizationData);
    }

    private void Start()
    {
        InitializeUIElements();

        currentBaseHealth = baseHealth;
        healthBar.HandleHealthChanged(currentBaseHealth);

        /*livesSlider.maxValue = baseStats.baseHealth;
        livesSlider.value = currentBaseHealth;*/

        waveManager = GetComponent<WaveManager>();

        canvas = UI.Canvas;

        moneyBaseColor = moneyCounterUI.color;
        materialBaseColor = materialCounterUI.color;

        UpdateUI();

        

        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    void InitializeUIElements()
    {
        Transform canvas = UI.Canvas.transform;
        
        Transform topPanel = canvas.GetChild(0);
        waveCounter = topPanel.Find("WaveHolder").Find("WaveCounter").gameObject;
        moneyCounterUI = topPanel.Find("MoneyHolder").Find("MoneyCounter").GetComponent<Text>();
        materialCounterUI = topPanel.Find("MaterialHolder").Find("MaterialCounter").GetComponent<Text>();

        

        healthBar = canvas.GetComponent<HealthBar>();
        livesSlider = canvas.Find("LivesSlider").gameObject;

        livesSlider.SetActive(true);

        victoryPanel = canvas.Find("VictoryPanel").gameObject;        
        defeatPanel = canvas.Find("DefeatPanel").gameObject;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Victory();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Defeat();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            money += 1000;
            material += 50;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (money - 1000 > 0 && material - 50 > 0)
            {
                money -= 1000;
                material -= 50;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CurrentWave = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CurrentWave = 5; 


        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            EventHandler.InvokeEvent(new SaveGameEvent("Debug Save"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CurrentWave = 6;


        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CurrentWave = 9;


        }
           
        UpdateUI();
    }

    void SaveGame(SaveGameEvent eventInfo)
    {
        print("Game saved");
        SaveData saveData = new SaveData(
            currentWave,
            enemiesKilled,
            moneyCollected,
            materialCollected,
            money,
            material,
            currentBaseHealth,
            currentScene: SceneManager.GetActiveScene().buildIndex,
            startingMode,
            towersPlaced,
            UpgradeController.currentUpgradeLevel
        );
        DataManager.WriteToFile(saveData, DataManager.SaveData);

        CustomizationData customData = new CustomizationData(
            Player1Color,
            Player2Color
        );
        DataManager.WriteToFile(customData, DataManager.CustomizationData);
    }

    public void TakeDamage(float damage, GameObject enemy)
    {
        damagingEnemy = enemy;
        currentBaseHealth -= damage;
        healthBar.HandleHealthChanged(currentBaseHealth);
        if (currentBaseHealth <= 0)
        {
            Defeat();
        }
    }

    // Modifiy the values to 10K if it is equal or higher than 10.000
    private void UpdateUI()
    {
        float mon = money;

        if(mon >= 10000)
        {
            int holeNumb = (int) mon / 10000;
            moneyCounterUI.text = holeNumb.ToString() + "K";
        }
        else
        {
            moneyCounterUI.text = mon.ToString();
        }

        float mat = material;

        if(mat >= 10000)
        {
            int holeNumb = (int) mat / 10000;
            materialCounterUI.text = holeNumb.ToString() + "K";
        }
        else
        {
            materialCounterUI.text = mat.ToString();
        }
    }

    public void AddMoney(float addMoney)
    {
        if (moneyParticle != null)
            moneyParticle.Play();

        money += addMoney;
        moneyCollected += (int) addMoney;
        UpdateUI();
    }

    public void AddMaterial(float addMaterial)
    {
        if(materialParticle != null)
            materialParticle.Play();

        material += addMaterial;
        materialCollected += (int) addMaterial;
        UpdateUI();
    }

    private IEnumerator currentMoneyCoroutine;
    private IEnumerator currentMaterialCoroutine;

    public bool SpendResources(float moneySpent, float materialSpent)
    {
        if (moneySpent <= money && materialSpent <= material)
        {
            if(moneySpent > 0)
            {
                money -= moneySpent;

                if (currentMoneyCoroutine != null)
                    StopCoroutine(currentMaterialCoroutine);

                currentMoneyCoroutine = DoColorBoughtFade(moneyCounterUI, moneyBaseColor, 2f);

                StartCoroutine(currentMoneyCoroutine);

                SpendEffektPrefab.GetComponentInChildren<Text>().text = moneySpent.ToString();

                Instantiate(SpendEffektPrefab, moneyCounterUI.transform);
            }
            
            if(materialSpent > 0)
            {
                material -= materialSpent;

                if (currentMaterialCoroutine != null)
                    StopCoroutine(currentMaterialCoroutine);

                currentMaterialCoroutine = DoColorBoughtFade(materialCounterUI, materialBaseColor, 2f);

                StartCoroutine(currentMaterialCoroutine);

                SpendEffektPrefab.GetComponentInChildren<Text>().text = materialSpent.ToString();

                Instantiate(SpendEffektPrefab, materialCounterUI.transform);
            }
            
            UpdateUI();
            return true;
        }
        //Show Error
        return false;
    }

    private IEnumerator DoColorBoughtFade(Text textUI, Color toColor, float duration)
    {
        float counter = 0f;

        textUI.color = Color.red;

        yield return new WaitForSeconds(1f);

        while (counter < duration)
        {
            counter += Time.deltaTime;
            textUI.color = Color.Lerp(textUI.color, toColor, counter / duration);

            yield return null;
        }
    }

    public bool CheckIfEnoughResources(Tower tower)
    {
        if (tower.cost <= money && tower.materialCost <= material)
        {
            return true;
        }
        return false;
    }

    public void AddPlacedTower(PlacedTower tower)
    {
        towersPlaced.Add(tower); 
    }

    public void RemovePlacedTower(GameObject tower)
    {
        PlacedTower clickedTower = TowerManager.Instance.GetPlacedTower(tower);
        towersPlaced.Remove(clickedTower);
    }

    private void Defeat()
    {
        Debug.Log("Defeat");

        EventHandler.InvokeEvent(new DefeatEvent(
            description: "Defeat",
            wave: currentWave + 1,
            killedBy: damagingEnemy,
            enemiesKilled: 0 
        ));

        canvas.GetComponent<UI>().SetSelectedButton("Restart");
        UI.OpenMenu();

        waveManager.Restart();

        livesSlider.SetActive(false);

        defeatPanel.SetActive(true);

        buildManager.TowerToBuild = null;

    }

    public void Victory()
    {
        Debug.Log("Victory");

        EventHandler.InvokeEvent(new VictoryEvent(
            description: "Victory",
            money: moneyCollected,
            material: materialCollected,
            enemiesKilled: enemiesKilled,
            towersBuilt: 0
        ));

        canvas.GetComponent<UI>().SetSelectedButton("Continue");
        UI.OpenMenu();

        waveManager.Restart();

        victoryPanel.SetActive(true);

        buildManager.TowerToBuild = null;
    }

    private void ResetBaseHealth()
    {
        currentBaseHealth = baseHealth;
        healthBar.HandleHealthChanged(currentBaseHealth);
    }
}

[Serializable]
public struct BaseStats
{
    public float money;
    public float material;
    public float baseHealth;
    public Color Player1Color;
    public Color Player2Color;
    public PlayerMode startingMode;
}
