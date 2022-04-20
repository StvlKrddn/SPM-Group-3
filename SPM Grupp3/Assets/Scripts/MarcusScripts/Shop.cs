using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public TowerPlacement towerPlacement;
    public GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void PurchaseCannonTower()
    {
        buildManager.TowerToBuild = buildManager.cannonTowerPrefab;

        towerPlacement = buildManager.ClickedArea.GetComponent<TowerPlacement>();

        print(buildManager.TowerToBuild.GetComponent<Tower>().cost);
        print(gameManager.Money);
        if (buildManager.TowerToBuild.GetComponent<Tower>().cost > gameManager.Money)
        {
            towerPlacement.SetStartColor();
            buildManager.ClickedArea = null;
            buildManager.TowerToBuild = null;
            return;
        }


        gameManager.spendResources(buildManager.TowerToBuild.GetComponent<Tower>().cost, 0);

        print(buildManager.TowerToBuild.GetComponent<Tower>().cost);
        print(gameManager.Money);

        Debug.Log("Cannon Tower");
        
        
        buildManager.InstantiateTower();

        

    }

    public void PurchaseMissileTower()
    {
        buildManager.TowerToBuild = buildManager.missileTowerPrefab;

        towerPlacement = buildManager.ClickedArea.GetComponent<TowerPlacement>();

        print(buildManager.TowerToBuild.GetComponent<Tower>().cost);
        print(gameManager.Money);

        if (!gameManager.spendResources(buildManager.TowerToBuild.GetComponent<Tower>().cost, 5) )
        {
            towerPlacement.SetStartColor();
            buildManager.ClickedArea = null;
            buildManager.TowerToBuild = null;
            return;
        }

     //   gameManager.spendResources(buildManager.TowerToBuild.GetComponent<Tower>().cost, 5);

        print(buildManager.TowerToBuild.GetComponent<Tower>().cost);
        print(gameManager.Money);

        Debug.Log("Missile Tower");
        
        buildManager.InstantiateTower();



        // Tower cost money
        /*if (buildManager.TowerToBuild.cost < gameManager.Money)
        {
            gameManager.spendResources(buildManager.TowerToBuild.cost, 0);
        }*/
    }
}
