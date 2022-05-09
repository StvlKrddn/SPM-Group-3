using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameManager gM;
    private Tower tower;

    [Header("UI Elements: ")]
    public Text moneyCostUI;
    public Text materialCostUI;

    private GameObject player;
    private Button pressedButton;
    private BuildManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        moneyCostUI.color = Color.gray;
        moneyCostUI.text = "0";
        materialCostUI.color = Color.gray;
        materialCostUI.text = "0";

        EventHandler.Instance.RegisterListener<UIClickedEvent>(OnClick);
    }

    public void PurchaseCannonTower()
    {
        buildManager = player.GetComponent<BuildManager>();
        buildManager.TowerToBuild = buildManager.cannonTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);

        print(buildManager.TowerToBuild);
    }

    public void PurchaseMissileTower()
    {
        buildManager = player.GetComponent<BuildManager>();
        buildManager.TowerToBuild = buildManager.missileTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);

        print(buildManager.TowerToBuild);
    }

    public void PurchaseSlowTower()
    {
        buildManager = player.GetComponent<BuildManager>();
        buildManager.TowerToBuild = buildManager.slowTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);

        print(buildManager.TowerToBuild);
    }

    public void PurchasePoisonTower()
    {
        buildManager = player.GetComponent<BuildManager>();
        buildManager.TowerToBuild = buildManager.poisonTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);

        print(buildManager.TowerToBuild);
    }

    public void OnClicked(Button button)
    {
        pressedButton = button;
        PurchaseTower();
    }

    public void OnClick(UIClickedEvent eventInfo)
    {
        player = eventInfo.Clicker;
    }

    private void PurchaseTower()
    {
        switch (pressedButton.name)
        {
            case "Cannon":
                PurchaseCannonTower();
                break;
            case "Missile":
                PurchaseMissileTower();
                break;
            case "Poison":
                PurchasePoisonTower();
                break;
            case "Slow":
                PurchaseSlowTower();
                break;
        }
    }

    private void UpdateCostUI(float moneyCost, float materialCost)
    {
        moneyCostUI.color = Color.red;
        moneyCostUI.text = "-" + moneyCost;

        if (materialCost > 0)
        {
            materialCostUI.color = Color.red;
            materialCostUI.text = "-" + materialCost; ;
        }
        else
        {
            materialCostUI.color = Color.gray;
            materialCostUI.text = "0";
        }
    }
}
