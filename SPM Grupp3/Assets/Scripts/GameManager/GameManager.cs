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
    [SerializeField] private Text moneyCounterUI;
    [SerializeField] private Text moneyChangerUI;
    [SerializeField] private Text materialCounterUI;
    [SerializeField] private Text materialChangerUI;
    [SerializeField] private Slider livesSlider;
    [SerializeField] private Color colorGain;

    [Header("Player")]
    [SerializeField] private PlayerMode startingMode;

    private int currentWave = -1;
    public float Money { get { return money; } set { money = value; } }
    public PlayerMode StartingMode { get { return startingMode; } }


    private void Start()
    {
        livesSlider.maxValue = baseHealth;
        materialChangerUI.text = "0";
        materialChangerUI.color = Color.gray;

        moneyChangerUI.text = "0";
        moneyChangerUI.color = Color.gray;
        UpdateResourcesUI();
    }

    void Update()
    {   
        if(Gamepad.current != null && Gamepad.current.xButton.isPressed && spawnEnemies)
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

        money += addMoney;
        UpdateResourcesUI();
    }

    public void AddMaterial(float addMaterial)
    {
        materialChangerUI.color = colorGain;
        materialChangerUI.text = "+" + addMaterial;

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

            material -= materialSpent;
            if (materialSpent > 0) 
            {
                materialChangerUI.color = Color.red;
                materialChangerUI.text = "-" + materialSpent;
            }
            else
            {
                materialChangerUI.color = Color.gray;
                materialChangerUI.text = "0";
            }
                

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
