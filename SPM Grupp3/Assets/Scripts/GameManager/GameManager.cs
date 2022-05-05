using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem; 

public class GameManager : MonoBehaviour
{

    [Header("Stats: ")]
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float material = 0f;
    [SerializeField] private float money = 350f;

    [Header("Enemies: ")]
    [SerializeField] public bool spawnEnemies = true;

    [Header("UI Elements: ")]
    [SerializeField] private Text moneyUI;
    [SerializeField] private Text materialUI;
    [SerializeField] private Slider livesSlider;

    [Header("Player")]
    [SerializeField] private PlayerMode startingMode;

    private int currentWave = -1;
    public float Money { get { return money; } set { money = value; } }
    public PlayerMode StartingMode { get { return startingMode; } }


    private void Start()
    {
        livesSlider.maxValue = baseHealth;
        UpdateResourcesUI();
    }

    void Update()
    {   
        

        if(Gamepad.current.xButton.isPressed && spawnEnemies )
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
        moneyUI.text = ": " + money;
        materialUI.text = ": " + material;
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

    public bool SpendResources(float moneySpent, float materialSpent)
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
