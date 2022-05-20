using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Stats: ")]
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float material = 0f;
    [SerializeField] private float money = 350f;

    [Header("UI: ")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject restartButton;

    [Header("Players: ")]
    [SerializeField] private PlayerMode startingMode;
    public Color Player1Color;
    public Color Player2Color;

    [Header("Other")]
    public List<GameObject> towersPlaced = new List<GameObject>();

    private BuildManager buildManager;
    private GameObject damagingEnemy;
    private WaveManager waveManager;
    private Canvas canvas;
    private Text moneyCounterUI;
    private Text materialCounterUI;
    private Slider livesSlider;
    private GameObject moneyUI;
    private GameObject materialUI;

    private int currentWave = -1;
    private float currentBaseHealth;
    private int enemiesKilled;
    private int moneyCollected;
    private int materialCollected;

    public float Money { get { return money; } set { money = value; } }
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

    private void Start()
    {
        InitializeUIElements();

        buildManager = FindObjectOfType<BuildManager>();
        currentBaseHealth = baseHealth;
        livesSlider.maxValue = currentBaseHealth;
        livesSlider.value = baseHealth;

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
        moneyCounterUI = currencyPanel.Find("MoneyHolder").Find("MoneyCounter").GetComponent<Text>();
        materialCounterUI = currencyPanel.Find("MaterialHolder").Find("MaterialCounter").GetComponent<Text>();
        
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

        GameObject player1 = GameObject.Find("Player");

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
    
    public void Continue()
    {
        ResetBaseHealth();
        victoryPanel.SetActive(false);
        GetComponent<PlayerManager>().Restart();
    }

    public void RestartGame()
    {
        money = 0;
        material = 0;
        UpdateResourcesUI();
        defeatPanel.SetActive(false);
        ResetBaseHealth();
        GetComponent<PlayerManager>().Restart();
    }

    private void ResetBaseHealth()
    {
        currentBaseHealth = baseHealth;
        livesSlider.value = 100;
    }
}
