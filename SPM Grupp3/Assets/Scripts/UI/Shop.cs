using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public GameManager gM;
    private Tower tower;
    /*    private Tower tower;*/

    [Header("UI Elements: ")]
    public Text moneyCostUI;
    public Text materialCostUI;

    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
        moneyCostUI.color = Color.gray;
        moneyCostUI.text = "0";
        materialCostUI.color = Color.gray;
        materialCostUI.text = "0";
    }

    public void PurchaseCannonTower()
    {

        buildManager.TowerToBuild = buildManager.cannonTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);

        print(buildManager.TowerToBuild);
/*        if (!gameManager.spendResources(tower.cost, 0))
        {
            towerPlacement.SetStartColor();
            buildManager.ClickedArea = null;
            buildManager.TowerToBuild = null;

            return;
        }*/
    }

    public void PurchaseMissileTower()
    {
        buildManager.TowerToBuild = buildManager.missileTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);

        print(buildManager.TowerToBuild);

        /*       if (!gameManager.spendResources(tower.cost, 5) )
                {

                    towerPlacement.SetStartColor();
                    buildManager.ClickedArea = null;
                    buildManager.TowerToBuild = null;
                    return;
                }*/


    }

    public void PurchaseSlowTower()
    {
        buildManager.TowerToBuild = buildManager.slowTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);

        print(buildManager.TowerToBuild);
        /*        if (buildManager.TowerToBuild.GetComponent<Tower>().cost > gameManager.Money)
                {
                    towerPlacement.SetStartColor();
                    buildManager.ClickedArea = null;
                    buildManager.TowerToBuild = null;
                    return;
                }*/
    }

    public void PurchasePoisonTower()
    {
        buildManager.TowerToBuild = buildManager.poisonTowerPrefab;
        UpdateCostUI(buildManager.TowerToBuild.GetComponent<Tower>().cost, buildManager.TowerToBuild.GetComponent<Tower>().materialCost);

        print(buildManager.TowerToBuild);
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
