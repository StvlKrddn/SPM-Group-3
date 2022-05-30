using System.Threading.Tasks;
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

    [SerializeField] private GameObject buildEffect;
    [SerializeField] private AudioClip buildSound;

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
            GameObject placedTower = BuildTower(TowerToBuild, ClickedArea.transform.GetChild(0).position, 0);

			ClickedArea = null;

            BuilderController buildController = transform.Find("BuilderMode").GetComponent<BuilderController>();
            buildController.purchasedTower = true;

            return;
        }
        
        print("Get away you are too poor!");
    }

    public void LoadTower(TowerData tower)
    {
        GameObject towerPrefab = GetTowerByType(tower.towerType);

        GameObject newTower = BuildTower(towerPrefab, tower.position, tower.level);
    }

    GameObject BuildTower(GameObject tower, Vector3 position, int level)
    {
        
        GameObject newTower = Instantiate(tower, position, Quaternion.identity);
/*        newTower.SetActive(false);*/
        Tower towerScript = newTower.GetComponent<Tower>();

        GameObject placement = FindTile(towerScript);
        Instantiate(towerBase, placement.transform.position, placement.transform.rotation, newTower.transform);
        placement.layer = LayerMask.NameToLayer("Road");
        towerScript.towerPlacement = placement;

        PlacedTower placedTower = new PlacedTower(newTower, towerScript.towerPlacement, level);

        // Måste vara GameManager.Instance då BuildTower ibland körs innan Start
        GameManager.Instance.AddPlacedTower(placedTower);

        EventHandler.InvokeEvent(new PlaySoundEvent("Tower is placed", buildSound));

        towerScript.radius.SetActive(false);
        if (level != 0)
        {
            towerScript.LoadTowerLevel(placedTower);
        }

        return newTower;
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
            if (c.gameObject.CompareTag("PlaceForTower"))
            {
                placement = c.gameObject;
            }
        }
        return placement;
    }
}
