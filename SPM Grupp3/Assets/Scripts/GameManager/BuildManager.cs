using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{    

    public GameObject cannonTowerPrefab;
    public GameObject missileTowerPrefab;
    public GameObject slowTowerPrefab;
    public GameObject poisonTowerPrefab;

    private GameManager gameManager;
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
        gameManager = GameManager.Instance;
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

        foreach (PlacedTower towerPlaced in gameManager.towersPlaced)
        {
            if (towerPlaced.tower == clickedArea)
            {
                return;
            }
        }

        if (gameManager.SpendResources(tower.cost, tower.materialCost))
        {
            BuildTower(TowerToBuild, ClickedArea.transform.GetChild(0).position, 0);

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

        BuildTower(towerPrefab, tower.position, tower.level);
    }

    private void BuildTower(GameObject tower, Vector3 position, int level)
    {
        GameObject newTower = Instantiate(tower, position, Quaternion.identity);
        Tower towerScript = newTower.GetComponent<Tower>();

        GameObject placement = FindTile(towerScript);
        Instantiate(towerBase, placement.transform.position, placement.transform.rotation, newTower.transform);
        placement.layer = LayerMask.NameToLayer("Road");
        towerScript.towerPlacement = placement;

        PlacedTower placedTower = new PlacedTower(newTower, towerScript.towerPlacement, level);

        // Måste vara GameManager.Instance då BuildTower ibland körs innan Start
        GameManager.Instance.AddPlacedTower(placedTower);

        towerScript.radius.SetActive(false);
        if (level != 0)
        {
            towerScript.LoadTowerLevel(placedTower);
        }
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
