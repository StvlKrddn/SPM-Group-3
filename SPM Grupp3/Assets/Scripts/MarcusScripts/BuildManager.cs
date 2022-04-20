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
    private GameObject clickedArea;
    public GameObject placedTower;
    private TowerPlacement towerPlacement;

    public GameObject TowerToBuild { get { return towerToBuild; } set { towerToBuild = value; } }
    public GameObject ClickedArea { get { return clickedArea; } set { clickedArea = value; } }

    private void Start()
    {
        EventHandler.Instance.RegisterListener<GarageEvent>(EnterBuildMode);
    }

    void OnDisable()
    {
        //EventHandler.Instance.UnregisterListener<GarageEvent>(EnterBuildMode);
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

    void EnterBuildMode(GarageEvent eventInfo)
    {
        print("Entered Build Mode!");
        //player.PlayerInput.SwitchCurrentActionMap("Builder");
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
