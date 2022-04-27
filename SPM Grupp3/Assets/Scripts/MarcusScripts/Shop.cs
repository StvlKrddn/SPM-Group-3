using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public TowerPlacement towerPlacement;
    public GameManager gameManager;
/*    private Tower tower;*/


    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
/*        tower = buildManager.TowerToBuild.GetComponent<Tower>();*/
    }

    public void PurchaseCannonTower()
    {
        buildManager.TowerToBuild = buildManager.cannonTowerPrefab;

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
    }
}
