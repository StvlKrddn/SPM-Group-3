using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one buildmanager");
        }
        instance = this;
    }

    [Header("Towers")] // Har enbart lagt till Header
    public GameObject cannonTowerPrefab;
    public GameObject missileTowerPrefab;

    private GameObject towerToBuild;

    public GameObject TowerToBuild { get { return towerToBuild; } set { towerToBuild = value; } }

    // Har enkelt satt två metoder som bestämmer vilken prefab som ska väljas med knappar på PlayerView
    public void ChooseCannon()
    {
        towerToBuild = cannonTowerPrefab;
    }

    public void ChooseMissile()
    {
        towerToBuild = missileTowerPrefab;
    }

/*    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }
    public void SetTowerToBuild(GameObject tower)
    {
        towerToBuild = tower;
    }*/
}
