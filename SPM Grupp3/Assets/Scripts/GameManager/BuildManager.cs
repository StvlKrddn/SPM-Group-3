using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{    
/*    public static BuildManager instance; */   

    private void Awake()
    {
        
/*        if (instance != null)
        {
            Debug.Log("More than one buildmanager");
        }
        instance = this;*/
    }

    public GameObject cannonTowerPrefab;
    public GameObject missileTowerPrefab;
    public GameObject slowTowerPrefab;
    public GameObject poisonTowerPrefab;

    private GameManager gM;
    private GameObject towerToBuild;
    private GameObject clickedArea;
    public GameObject placedTower;
    private TowerPlacement towerPlacement;
    private Tower tower;
    [SerializeField] private GameObject towerBase;

    public TowerPlacement TowerPlacement { get { return towerPlacement; } set { towerPlacement = value; } }
    public GameObject TowerToBuild { get { return towerToBuild; } set { towerToBuild = value; } }
    public GameObject ClickedArea { get { return clickedArea; } set { clickedArea = value; } }

    private void Start()
    {
        gM = GameManager.Instance;
    }

    void OnDisable()
    {
        //EventHandler.Instance.UnregisterListener<GarageEvent>(EnterBuildMode);
    }



    public void InstantiateTower()
    {
        if (TowerToBuild != null)
        {
            tower = TowerToBuild.GetComponent<Tower>();
        }
        
        if (clickedArea == null)
        {
            return;
        }

        foreach (GameObject towerPlaced in gM.towersPlaced)
        {
            if (towerPlaced == clickedArea)
            {
                return;
            }
        }

        if (gM.SpendResources(tower.cost, tower.materialCost))
        {
            placedTower = Instantiate(TowerToBuild, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation);
			Instantiate(towerBase, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation, placedTower.transform);
			placedTower.GetComponent<Tower>().towerPlacement = ClickedArea;
			clickedArea.layer = 11;

			ClickedArea = null;

            TowerUpgradeCotroller.instance.PlaceTowerInUpgradeList(placedTower);
            gM.AddPlacedTower(placedTower);

            return;
        }
        
        print("Get away you are too poor!");
    }
}
