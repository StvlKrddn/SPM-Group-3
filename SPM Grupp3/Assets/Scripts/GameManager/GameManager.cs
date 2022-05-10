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

    [Header("Enemies: ")]
    [SerializeField] public bool spawnEnemies = true;

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

    private int currentWave = -1;
    public float Money { get { return money; } set { money = value; } }
    public PlayerMode StartingMode { get { return startingMode; } }

    private void Start()
    {
        livesSlider.maxValue = baseHealth;

        UpdateResourcesUI();

        EventHandler.Instance.RegisterListener<StartWaveEvent>(OnStartWave);
    }

    void Update()
    {  
        /*
        if(Gamepad.current != null && Gamepad.current.xButton.isPressed && spawnEnemies)
        {
            SpawnWave();
        }
        */
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
        // Invoke new wave event
        EventHandler.Instance.InvokeEvent(new NewWaveEvent(
            description: "New wave started",
            currentWave: currentWave
            ));

        spawnEnemies = false;
        //Deactivate start button
    }

    public void TakeDamage(float damage)
    {
        baseHealth -= damage;
        livesSlider.value -= damage;
        if (baseHealth <= 0)
        {
            GameObject fillArea = livesSlider.transform.Find("Fill Area").gameObject;
            fillArea.SetActive(false);
            Defeat();
        }
        UpdateResourcesUI();
    }

    private void UpdateResourcesUI()
    {
        moneyCounterUI.text = ": " + money;
        materialCounterUI.text = ": " + material;
    }

    public void AddMoney(float addMoney)
    {
        moneyChangerUI.color = colorGain;
        moneyChangerUI.text = "+" + addMoney;

        //Instantiate(moneyChangerUI, moneyUI.transform);

        money += addMoney;
        UpdateResourcesUI();
    }

    public void AddMaterial(float addMaterial)
    {
        materialChangerUI.color = colorGain;
        materialChangerUI.text = "+" + addMaterial;

        //Instantiate(materialChangerUI, materialUI.transform);

        material += addMaterial;
        UpdateResourcesUI();
    }

    public bool SpendResources(float moneySpent, float materialSpent)
    {
        if (moneySpent <= money && materialSpent <= material)
        {
            money -= moneySpent;
            moneyChangerUI.color = Color.red;
            moneyChangerUI.text = "-" + moneySpent;

            //Instantiate(moneyChangerUI, moneyUI.transform);

            material -= materialSpent;
            if (materialSpent > 0) 
            {
                materialChangerUI.color = Color.red;
                materialChangerUI.text = "-" + materialSpent;

                //Instantiate(materialChangerUI, moneyUI.transform);
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
    }

}
