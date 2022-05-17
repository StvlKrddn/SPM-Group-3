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

    [Header("Vet inte vad detta är: ")]
    [SerializeField] private Color colorGain;

    [Header("Player")]
    [SerializeField] private PlayerMode startingMode;

    [Header("Other")]
    public List<GameObject> towersPlaced = new List<GameObject>();

    public GameObject selectedButton;

    private BuildManager buildManager;
    private GameObject damagingEnemy;
    private WaveManager waveManager;
    private Canvas canvas;
    private Text moneyCounterUI;
    private Text moneyChangerUI;
    private Text materialCounterUI;
    private Text materialChangerUI;
    private Slider livesSlider;
    private GameObject victoryUI;
    private GameObject defeatUI;
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

        victoryUI.SetActive(false);
        defeatUI.SetActive(false);
    }

    void InitializeUIElements()
    {
        Transform canvas = UI.Canvas.transform;
        
        Transform currencyPanel = canvas.GetChild(0);
        moneyCounterUI = currencyPanel.Find("MoneyHolder").Find("MoneyCounter").GetComponent<Text>();
        moneyUI = currencyPanel.Find("MoneyChanger").gameObject;
        moneyChangerUI = moneyUI.transform.GetChild(0).GetComponent<Text>();
        materialCounterUI = currencyPanel.Find("MaterialHolder").Find("MaterialCounter").GetComponent<Text>();
        materialUI = currencyPanel.Find("MaterialChanger").gameObject;
        materialChangerUI = materialUI.transform.GetChild(0).GetComponent<Text>();
        
        livesSlider = canvas.Find("LivesSlider").GetComponent<Slider>();

        defeatUI = canvas.Find("DefeatPanel").gameObject;
        victoryUI = canvas.Find("VictoryPanel").gameObject;
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
        materialCounterUI.text = ": " + material;
    }

    public void AddMoney(float addMoney)
    {
        moneyChangerUI.color = colorGain;
        moneyChangerUI.text = "+" + addMoney;

        Instantiate(moneyChangerUI, moneyUI.transform);

        money += addMoney;
        moneyCollected += (int) addMoney;
        UpdateResourcesUI();
    }

    public void AddMaterial(float addMaterial)
    {
        materialChangerUI.color = colorGain;
        materialChangerUI.text = "+" + addMaterial;

        Instantiate(materialChangerUI, materialUI.transform);

        material += addMaterial;
        materialCollected += (int) addMaterial;
        UpdateResourcesUI();
    }

    public bool SpendResources(float moneySpent, float materialSpent)
    {
        if (moneySpent <= money && materialSpent <= material)
        {
            money -= moneySpent;
            moneyChangerUI.color = Color.red;
            moneyChangerUI.text = "-" + moneySpent;

            Instantiate(moneyChangerUI, moneyUI.transform);

            material -= materialSpent;
            if (materialSpent > 0) 
            {
                materialChangerUI.color = Color.red;
                materialChangerUI.text = "-" + materialSpent;

                Instantiate(materialChangerUI, materialUI.transform);
            }
               

            UpdateResourcesUI();
            return true;
        }
        //Show Error
        return false;
    }

    public bool CheckIfEnoughResourcesForTower()
    {
        Tower tower = buildManager.TowerToBuild.GetComponent<Tower>();
        if (tower.cost <= money && tower.materialCost <= material)
        {
            return true;
        }
        //Show Error
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

        UI.OpenMenu();

        waveManager.Restart();
       
        defeatUI.SetActive(true);

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

        UI.OpenMenu();

        waveManager.Restart();

        victoryUI.SetActive(true);

        buildManager.TowerToBuild = null;

        GameObject.Find("Player").GetComponent<PlayerHandler>().SetSelected(selectedButton);
    }
    
    public void Continue()
    {
        ResetBaseHealth();
        victoryUI.SetActive(false);
        GetComponent<PlayerManager>().Restart();
    }

    public void RestartGame()
    {
        money = 0;
        material = 0;
        UpdateResourcesUI();
        defeatUI.SetActive(false);
        ResetBaseHealth();
        GetComponent<PlayerManager>().Restart();
    }

    private void ResetBaseHealth()
    {
        currentBaseHealth = baseHealth;
        livesSlider.value = 100;
    }
}
