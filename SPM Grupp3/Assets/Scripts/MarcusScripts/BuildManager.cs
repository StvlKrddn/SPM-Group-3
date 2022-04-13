using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Transform garage;
    
    public static BuildManager instance;
    
    private GarageTrigger garageTrigger;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one buildmanager");
        }
        instance = this;
    }

    public GameObject cannonTowerPrefab;
    public GameObject missileTowerPrefab;


    private GameObject towerToBuild;

    public GameObject TowerToBuild { get { return towerToBuild; } set { towerToBuild = value; } }

/*    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }
    public void SetTowerToBuild(GameObject tower)
    {
        towerToBuild = tower;
    }*/
}
