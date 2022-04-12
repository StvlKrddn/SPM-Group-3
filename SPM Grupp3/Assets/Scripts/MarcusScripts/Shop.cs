using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    TowerPlacement towerPlacement;

    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void PurchaseCannonTower()
    {
        Debug.Log("Cannon Tower");
        buildManager.TowerToBuild = buildManager.cannonTowerPrefab;
        /*towerPlacement.InstantiateTower();*/
    }

    public void PurchaseMissileTower()
    {
        Debug.Log("Missile Tower");
        buildManager.TowerToBuild = buildManager.missileTowerPrefab;
        /*towerPlacement.InstantiateTower();*/
    }
}
