using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    TowerPlacement towerPlacement;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void PurchaseCannonTower()
    {
        Debug.Log("Cannon Tower");
        buildManager.TowerToBuild = buildManager.cannonTowerPrefab.GetComponent<Tower>();
        towerPlacement.InstantiateTower();

        // Tower cost money
        /*if (buildManager.TowerToBuild.cost < gameManager.Money)
        {
            gameManager.spendResources(buildManager.TowerToBuild.cost, 0);
        }*/
    }

    public void PurchaseMissileTower()
    {
        Debug.Log("Missile Tower");
        buildManager.TowerToBuild = buildManager.missileTowerPrefab.GetComponent<Tower>();
        towerPlacement.InstantiateTower();

        // Tower cost money
        /*if (buildManager.TowerToBuild.cost < gameManager.Money)
        {
            gameManager.spendResources(buildManager.TowerToBuild.cost, 0);
        }*/
    }
}
