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

    public GameObject cannonTowerPrefab;
    public GameObject missileTowerPrefab;
    public GameObject slowTowerPrefab;
    public GameObject poisonTowerPrefab;

    private GameObject towerToBuild;
    private GameObject clickedArea;
    public GameObject placedTower;
    private TowerPlacement towerPlacement;

    public GameObject TowerToBuild { get { return towerToBuild; } set { towerToBuild = value; } }
    public GameObject ClickedArea { get { return clickedArea; } set { clickedArea = value; } }

    private void Start()
    {
        
    }

    public void InstantiateTower()
    {
        /*       GameObject towerToBuild = TowerToBuild;*/
        //if (gameManager.Money < towerToBuild)
        if (clickedArea == null)
        {
            return;
        }
        towerPlacement = clickedArea.GetComponent<TowerPlacement>();
        placedTower = Instantiate(TowerToBuild, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation);
        towerPlacement.SetDoNotHover();
        towerPlacement.SetStartColor();
        ClickedArea = null;
        TowerToBuild = null;
        /*UI.SetActive(false);*/
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
