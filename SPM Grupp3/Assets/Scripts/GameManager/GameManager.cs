using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //[Header("Stats: ")]
    //[SerializeField] private float baseHealth = 100f;
    //[SerializeField] private float material = 0f;
    //[SerializeField] private float money = 350f;
    [SerializeField] private BaseStats baseStats;

    [Header("UI: ")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject restartButton;

    //[Header("Players: ")]
    //[SerializeField] private PlayerMode startingMode;
    [NonSerialized] public Color Player1Color;
    [NonSerialized] public Color Player2Color;

    [Header("Other")]
    [NonSerialized] public List<GameObject> towersPlaced = new List<GameObject>();

    private BuildManager buildManager;
    private GameObject damagingEnemy;
    private WaveManager waveManager;
    private Canvas canvas;
    [SerializeField] private Text moneyCounterUI;
    [SerializeField] private Text materialCounterUI;
    private Slider livesSlider;

    private float money;
    private float material;
    private float baseHealth;
    private PlayerMode startingMode;

    private int currentWave = -1;
    private float currentBaseHealth;
    private int enemiesKilled;
    private int moneyCollected;
    private int materialCollected;
    public PlayerMode StartingMode { get { return startingMode; } }
    public int EnemiesKilled { get { return enemiesKilled; } set { enemiesKilled = value; } }

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
        EventHandler.Instance.RegisterListener<SaveGameEvent>(SaveGame);
        if (DataManager.FileExists(DataManager.SaveData))
        {
            LoadFromFile();
        }
        else 
        {
            LoadBase();
        }
    }

    private void LoadFromFile()
    {
        SaveData data = (SaveData) DataManager.ReadFromFile(DataManager.SaveData);
        money = data.money;
        material = data.material;
        currentWave = data.currentWave;
        baseHealth = data.currentBaseHealth;
        enemiesKilled = data.enemiesKilled;
        moneyCollected = data.moneyCollected;
        materialCollected = data.materialCollected;
    }

    private void LoadBase()
    {
        money = baseStats.money;
        material = baseStats.material;
        currentWave = -1;
        baseHealth = baseStats.baseHealth;
        
        enemiesKilled = 0;
        moneyCollected = 0;
        materialCollected = 0;

        Player1Color = baseStats.Player1Color;
        Player2Color = baseStats.Player2Color;
    }

    public void DeleteSaveData()
    {
        DataManager.DeleteFile(DataManager.SaveData);
    }

    private void Start()
    {
        InitializeUIElements();

        buildManager = FindObjectOfType<BuildManager>();
        currentBaseHealth = baseHealth;
        livesSlider.maxValue = baseStats.baseHealth;
        livesSlider.value = currentBaseHealth;

        waveManager = GetComponent<WaveManager>();

        canvas = UI.Canvas;

        UpdateResourcesUI();

        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    void InitializeUIElements()
    {
        Transform canvas = UI.Canvas.transform;
        
        Transform currencyPanel = canvas.GetChild(0);
        //moneyCounterUI = currencyPanel.Find("MoneyHolder").Find("MoneyCounter").GetComponent<Text>();
        //materialCounterUI = currencyPanel.Find("MaterialHolder").Find("MaterialCounter").GetComponent<Text>();
        
        livesSlider = canvas.Find("LivesSlider").GetComponent<Slider>();

        defeatPanel = canvas.Find("DefeatPanel").gameObject;
        victoryPanel = canvas.Find("VictoryPanel").gameObject;
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
        UpdateResourcesUI();
    }

    void SaveGame(SaveGameEvent eventInfo)
    {
        print("Game manager saved!");
        SaveData saveData = new SaveData(
            currentWave,
            enemiesKilled,
            moneyCollected,
            materialCollected,
            money,
            material,
            currentBaseHealth,
            currentScene: SceneManager.GetActiveScene().buildIndex
        );
        DataManager.WriteToFile(saveData, DataManager.SaveData);
    }

    public void TakeDamage(float damage, GameObject enemy)
    {
        damagingEnemy = enemy;
        currentBaseHealth -= damage;
        livesSlider.value -= damage;
        if (currentBaseHealth <= 0)
        {
            Defeat();
        }
    }

    private void UpdateResourcesUI()
    {
        float mon;
        mon = money;
        /*if (money / 1000 >= 1)
        {
            mon = money / 1000;
            Mathf.Round(mon);
            moneyCounterUI.text = mon.ToString() + " K";
            materialCounterUI.text = ": " + material;
            return;
        }*/

        moneyCounterUI.text = ": " + mon.ToString();
        materialCounterUI.text = ": " + material.ToString();
    }

    public void AddMoney(float addMoney)
    {
/*        moneyChangerUI.color = colorGain;
        moneyChangerUI.text = "+" + addMoney;

        Instantiate(moneyChangerUI, moneyUI.transform);*/

        money += addMoney;
        moneyCollected += (int) addMoney;
        UpdateResourcesUI();
    }

    public void AddMaterial(float addMaterial)
    {
/*        materialChangerUI.color = colorGain;
        materialChangerUI.text = "+" + addMaterial;

        Instantiate(materialChangerUI, materialUI.transform);*/

        material += addMaterial;
        materialCollected += (int) addMaterial;
        UpdateResourcesUI();
    }

    public bool SpendResources(float moneySpent, float materialSpent)
    {
        if (moneySpent <= money && materialSpent <= material)
        {
            money -= moneySpent;
/*            moneyChangerUI.color = Color.red;
            moneyChangerUI.text = "-" + moneySpent;*//*

            Instantiate(moneyChangerUI, moneyUI.transform);*/

            material -= materialSpent;
/*            if (materialSpent > 0) 
            {
                materialChangerUI.color = Color.red;
                materialChangerUI.text = "-" + materialSpent;

                Instantiate(materialChangerUI, materialUI.transform);
            }*/
               

            UpdateResourcesUI();
            return true;
        }
        //Show Error
        return false;
    }

    public bool CheckIfEnoughResources(Tower tower)
    {
        if (tower.cost < money && tower.materialCost < material)
        {
            return true;
        }
        return false;
    }

    public void AddPlacedTower(GameObject tower)
    {
        towersPlaced.Add(tower); 
    }

    private void Defeat()
    {
        Debug.Log("Defeat");

        EventHandler.Instance.InvokeEvent(new DefeatEvent(
            description: "Defeat",
            wave: currentWave + 1,
            killedBy: damagingEnemy,
            enemiesKilled: 0 
        ));

        canvas.GetComponent<UI>().SetSelectedButton(restartButton);
        UI.OpenMenu();

        waveManager.Restart();
       
        defeatPanel.SetActive(true);

        buildManager.TowerToBuild = null;

    }

    public void Victory()
    {
        Debug.Log("Victory");

        // NOTE(August): Lite stats som kan vara kul att ha med på Victory Panel
        /*print("Money collected: " + moneyCollected);
        print("Material collected: " + materialCollected);
        print("Enemies killed: " + enemiesKilled);*/

        EventHandler.Instance.InvokeEvent(new VictoryEvent(
            description: "Victory",
            money: (int) moneyCollected,
            material: (int) materialCollected,
            enemiesKilled: enemiesKilled,
            towersBuilt: 0
        ));

        canvas.GetComponent<UI>().SetSelectedButton(continueButton);
        UI.OpenMenu();

        waveManager.Restart();

        victoryPanel.SetActive(true);

        buildManager.TowerToBuild = null;
    }

    private void ResetBaseHealth()
    {
        currentBaseHealth = baseHealth;
        livesSlider.value = currentBaseHealth;
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
