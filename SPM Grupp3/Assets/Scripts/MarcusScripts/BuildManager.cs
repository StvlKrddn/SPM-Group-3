using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{    
    public static BuildManager instance;    
    [SerializeField] private GameManager gM;

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
    private Tower tower;
    [SerializeField] private GameObject towerBase;


    public List<GameObject> towersPlaced = new List<GameObject>(); 

    public GameObject TowerToBuild { get { return towerToBuild; } set { towerToBuild = value; } }
    public GameObject ClickedArea { get { return clickedArea; } set { clickedArea = value; } }

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
        if (gM.spendResources(tower.cost, tower.materialCost))
        {
            towerPlacement = clickedArea.GetComponent<TowerPlacement>();
            Instantiate(towerBase, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation);
            placedTower = Instantiate(TowerToBuild, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation);
            towersPlaced.Add(placedTower);
            towerPlacement.SetDoNotHover();
            towerPlacement.SetStartColor();
            ClickedArea = null;
            return;
        }
        print("Get away you are too poor!");
    }
}
