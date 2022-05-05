using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public GameManager gM;
    private Tower tower;
/*    private Tower tower;*/


    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void PurchaseCannonTower()
    {

        buildManager.TowerToBuild = buildManager.cannonTowerPrefab;     
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
        print(buildManager.TowerToBuild);
    }
}
