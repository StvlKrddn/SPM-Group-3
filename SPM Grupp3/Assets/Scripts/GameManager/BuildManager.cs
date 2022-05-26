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
        //EventHandler.UnregisterListener<GarageEvent>(EnterBuildMode);
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

        foreach (PlacedTower towerPlaced in gM.towersPlaced)
        {
            if (towerPlaced.tower == clickedArea)
            {
                return;
            }
        }

        if (gM.SpendResources(tower.cost, tower.materialCost))
        {
            placedTower = Instantiate(TowerToBuild, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation);
			Instantiate(towerBase, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation, placedTower.transform);
            Tower tower = placedTower.GetComponent<Tower>();
			tower.towerPlacement = ClickedArea;
			clickedArea.layer = 11;
            tower.radius.SetActive(false);
			ClickedArea = null;

            gM.AddPlacedTower(new PlacedTower(placedTower, tower.towerPlacement, 0));
            BuilderController buildController = transform.Find("BuilderMode").GetComponent<BuilderController>();
            buildController.purchasedTower = true;



            return;
        }
        
        print("Get away you are too poor!");
    }

    public PlacedTower LoadTower(TowerData tower)
    {
        GameObject towerPrefab = GetTowerByType(tower.towerType);
        GameObject newTower = Instantiate(towerPrefab, tower.position, Quaternion.identity);

        Tower towerScript = newTower.GetComponent<Tower>();
        GameObject placement = FindTile(towerScript);
        placement.layer = LayerMask.NameToLayer("Road");

        Instantiate(towerBase, placement.transform.position, placement.transform.rotation, newTower.transform);

        PlacedTower placedTower = new PlacedTower(newTower, towerScript.towerPlacement, tower.level);
        towerScript.radius.SetActive(false);
        towerScript.LoadTowerLevel(placedTower);
        return placedTower;
    }

    GameObject GetTowerByType(string towerType)
    {
        switch (towerType)
        {
            case "CannonTower(Clone)":
                return cannonTowerPrefab;
            case "MissileTower(Clone)":
                return missileTowerPrefab;
            case "SlowTower(Clone)":
                return slowTowerPrefab;
            case "PoisonTower(Clone)":
                return poisonTowerPrefab;
            default:
                return null;
        }
    }

    GameObject FindTile(Tower tower)
    {
        GameObject placement = null;
        Collider[] colliders = Physics.OverlapBox(tower.transform.position, transform.localScale / 1.5f);
        foreach (Collider c in colliders)
        {
            int layer = c.gameObject.layer;
            if (layer == LayerMask.NameToLayer("PlaceForTower"))
            {
                placement = c.gameObject;
            }
        }
        return placement;
    }
}
