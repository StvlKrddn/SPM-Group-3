using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private GameManager gM;
    private GameObject towerToBuild;
    private GameObject clickedArea;
    public GameObject placedTower;
    private TowerPlacement towerPlacement;
    private Tower tower;
    [SerializeField] private GameObject towerBase;


    public List<GameObject> towersPlaced = new List<GameObject>(); 

    public GameObject TowerToBuild { get { return towerToBuild; } set { towerToBuild = value; } }
    public GameObject ClickedArea { get { return clickedArea; } set { clickedArea = value; } }

    private void Start()
    {
        gM = FindObjectOfType<GameManager>();
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
        if (gM.SpendResources(tower.cost, tower.materialCost))
        {
            towerPlacement = clickedArea.GetComponent<TowerPlacement>();
            Instantiate(towerBase, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation);
            placedTower = Instantiate(TowerToBuild, ClickedArea.transform.GetChild(0).transform.position, ClickedArea.transform.GetChild(0).transform.rotation);
/*            placedTower.GetComponent<Tower>().CheckLevels();*/
            towersPlaced.Add(placedTower);
            towerPlacement.SetDoNotHover();
            towerPlacement.SetStartColor();
            ClickedArea = null;
            return;
        }
        print("Get away you are too poor!");
    }
}
