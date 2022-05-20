using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("UI Elements: ")]
    public Canvas canvas;

    private Button pressedButton;
    private BuildManager buildManager;

    void Start()
    {
        Camera canvasCamera = GameObject.FindGameObjectWithTag("Camera/CanvasCamera").GetComponent<Camera>();
        canvas.worldCamera = canvasCamera;
    }

    public void PurchaseCannonTower()
    {
        buildManager = GetComponentInParent<BuildManager>();
        buildManager.TowerToBuild = buildManager.cannonTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);
        buildManager.InstantiateTower();
    }

    public void PurchaseMissileTower()
    {
        buildManager = GetComponentInParent<BuildManager>();
        buildManager.TowerToBuild = buildManager.missileTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);
        buildManager.InstantiateTower();
    }

    public void PurchaseSlowTower()
    {
        buildManager = GetComponentInParent<BuildManager>();
        buildManager.TowerToBuild = buildManager.slowTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);
        buildManager.InstantiateTower();
    }

    public void PurchasePoisonTower()
    {
        buildManager = GetComponentInParent<BuildManager>();
        buildManager.TowerToBuild = buildManager.poisonTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);
        buildManager.InstantiateTower();
    }

    public void OnClicked(Button button)
    {
        pressedButton = button;
        PurchaseTower();
    }

/*    public void OnClick(UIClickedEvent eventInfo)
    {
        player = eventInfo.Clicker;
    }*/

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
/*        moneyCostUI.color = Color.red;
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
        }*/
    }
}
