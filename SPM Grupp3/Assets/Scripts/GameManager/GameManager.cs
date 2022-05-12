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
    [SerializeField] private Text materialCounterUI;
    [SerializeField] private Text materialChangerUI;
    [SerializeField] private Slider livesSlider;
    [SerializeField] private Color colorGain;

    [Header("Spanwer for UI Elements")]
    [SerializeField] private GameObject moneyUI;
    [SerializeField] private GameObject materialUI;

    [Header("Player")]
    [SerializeField] private PlayerMode startingMode;

    [Header("Other")]
    public List<GameObject> towersPlaced = new List<GameObject>();

    private GameObject damagingEnemy;
    private WaveManager waveManager;

    private int currentWave = -1;
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
        livesSlider.maxValue = baseHealth;

        waveManager = GetComponent<WaveManager>();

        UpdateResourcesUI();

        
    }

    public void TakeDamage(float damage, GameObject enemy)
    {
        damagingEnemy = enemy;
        baseHealth -= damage;
        livesSlider.value -= damage;
        if (baseHealth <= 0)
        {
            GameObject fillArea = livesSlider.transform.Find("Fill Area").gameObject;
            fillArea.SetActive(false);
            Defeat();
        }
    }

    private void UpdateResourcesUI()
    {
        float mon;
        mon = money;
        if (money / 1000 >= 1)
        {
            mon = money / 1000;
            Mathf.Round(mon);
            moneyCounterUI.text = mon.ToString() + " K";
            materialCounterUI.text = ": " + material;
            return;
        }

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

        EventHandler.Instance.InvokeEvent(new DefeatEvent(
            description: "Defeat",
            wave: currentWave + 1,
            killedBy: damagingEnemy,
            enemiesKilled: 0 
        ));

        // NOTE(August): Allt under detta borde egentligen hända efter man trycker på Restart-knappen i Defeat skärmen
        // Kanske ska flyttas till nån OnClick-metod någonstans eller nått
        money = 0;
        material = 0;
        UpdateResourcesUI();
        waveManager.Restart();
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
    }
}
