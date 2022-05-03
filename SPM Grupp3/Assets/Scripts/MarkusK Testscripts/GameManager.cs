using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{

    [Header("Stats: ")]
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float material = 0f;
    [SerializeField] private float money = 350f;

    [Header("Enemies: ")]
    [SerializeField] public bool spawnEnemies = true;

    [Header("UI Elements: ")]
    [SerializeField] private Text waveUI;
    [SerializeField] private Text moneyUI;
    [SerializeField] private Text materialUI;
    [SerializeField] private Slider livesSlider;

    [Header("Player")]
    [SerializeField] private PlayerMode startingMode;

    private int currentWave = 0;
    private int victoryWave = 10;
    public float Money { get { return money; } set { money = value; } }
    public PlayerMode StartingMode { get { return startingMode; } }


    private void Start()
    {
        livesSlider.maxValue = baseHealth;
        UpdateResourcesUI();
    }

    void Update()
    {
        
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

    // Har enbart ändrat på texten metoden nedan
    private void UpdateResourcesUI()
    {
        moneyUI.text = ": " + money;
        materialUI.text = ": " + material;
        waveUI.text = currentWave + "/9";
    }

    public void AddMoney(float addMoney)
    {
        money += addMoney;
        UpdateResourcesUI();
    }

    public void AddMaterial(float addMaterial)
    {
        material += addMaterial;
        UpdateResourcesUI();
    }

    public bool spendResources(float moneySpent, float materialSpent) //Change to big S
    {
        if (moneySpent <= money && materialSpent <= material)
        {
            money -= moneySpent;
            material -= materialSpent;
            UpdateResourcesUI();
            return true;
        }
        //Show Error
        return false;
    }

    private void Defeat()
    {
        Debug.Log("Defeat");
    }

}
