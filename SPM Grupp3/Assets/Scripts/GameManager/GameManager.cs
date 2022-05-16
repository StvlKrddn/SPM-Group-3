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

    [Header("UI Elements: ")]
    [SerializeField] private Text moneyCounterUI;
    [SerializeField] private Text moneyChangerUI;
    [Space]
    [SerializeField] private Text materialCounterUI;
    [SerializeField] private Text materialChangerUI;
    [SerializeField] private Slider livesSlider;
    [Space]
    [SerializeField] private Color colorGain;
    [Space]
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject defeatUI;

    [Header("Spanwer for UI Elements")]
    [SerializeField] private GameObject moneyUI;
    [SerializeField] private GameObject materialUI;

    [Header("Player")]
    [SerializeField] private PlayerMode startingMode;

    [Header("Other")]
    public List<GameObject> towersPlaced = new List<GameObject>();

    private BuildManager buildManager;
    private GameObject damagingEnemy;
    private WaveManager waveManager;

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
        buildManager = FindObjectOfType<BuildManager>();
        currentBaseHealth = baseHealth;
        livesSlider.maxValue = currentBaseHealth;
        livesSlider.value = baseHealth;

        waveManager = GetComponent<WaveManager>();

        UpdateResourcesUI();

        victoryUI.SetActive(false);
        defeatUI.SetActive(false);
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

    public void AddPlacedTower(GameObject tower)
    {
        towersPlaced.Add(tower); 
    }

    private void Defeat()
    {
        Debug.Log("Defeat");

        waveManager.Restart();

        EventHandler.Instance.InvokeEvent(new DefeatEvent(
            description: "Defeat",
            wave: currentWave + 1,
            killedBy: damagingEnemy,
            enemiesKilled: 0 
        ));

        GameObject player1 = GameObject.Find("Player");

        GetComponent<PlayerManager>().TurnOnCursor();
       
        defeatUI.SetActive(true);

        buildManager.TowerToBuild = null;

        Time.timeScale = 0;
    }

    private void ResetBaseHealth()
    {
        currentBaseHealth = baseHealth;
        livesSlider.value = 100;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        money = 0;
        material = 0;
        UpdateResourcesUI();
        defeatUI.SetActive(false);
        ResetBaseHealth();
        GetComponent<PlayerManager>().Restart();
    }

    public void Victory()
    {
        Debug.Log("Victory");
        print("Money collected: " + moneyCollected);
        print("Material collected: " + materialCollected);
        print("Enemies killed: " + enemiesKilled);

        EventHandler.Instance.InvokeEvent(new VictoryEvent(
            description: "Victory",
            money: (int) moneyCollected,
            material: (int) materialCollected,
            enemiesKilled: enemiesKilled,
            towersBuilt: 0
        ));

        waveManager.Restart();

        GetComponent<PlayerManager>().TurnOnCursor();

        victoryUI.SetActive(true);

        buildManager.TowerToBuild = null;

        Time.timeScale = 0;
    }
    
    public void Continue()
    {
        Time.timeScale = 1;
        ResetBaseHealth();
        victoryUI.SetActive(false);
        GetComponent<PlayerManager>().Restart();
    }
}
