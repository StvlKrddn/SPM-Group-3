using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacedTower
{
    public GameObject tower;
    public int upgradesPurchased;
    public GameObject towerPlacement;
    public PlacedTower(GameObject t, GameObject towerPlacement, int uP)
    {
        tower = t;
        this.towerPlacement = towerPlacement;
        upgradesPurchased = uP;
    }
}
public class TowerManager : MonoBehaviour
{
    private static TowerManager instance;
    private GameObject clickedTower;
    private GameManager gameManager;
    private List<PlacedTower> placedTowers;

    public GameObject ClickedTower { get { return clickedTower; } set { clickedTower = value; } }
    public static TowerManager Instance 
    { 
        get 
        {
            // "Lazy loading" to prevent Unity load order error
            if (instance == null)
            {
                instance = FindObjectOfType<TowerManager>();
            }
            return instance; 
        } 
    }

    void Awake()
    {
        gameManager = GameManager.Instance;
        placedTowers = gameManager.towersPlaced;

        EventHandler.RegisterListener<TowerClickedEvent>(GetTowerClicked);
    }

    public void GetTowerClicked(TowerClickedEvent eventInfo)
    {
        clickedTower = eventInfo.towerClicked;
    }

    public int GetUpgradesPurchased()
    {
        for (int i = 0; i < placedTowers.Count; i++)
        {
            if (placedTowers[i].tower.Equals(clickedTower))
            {
                return placedTowers[i].upgradesPurchased;
            }
        }
        return 0;
    }

    public int GetUpgradesPurchased(GameObject tower)
    {
        for (int i = 0; i < placedTowers.Count; i++)
        {
            if (placedTowers[i].tower.Equals(tower))
            {
                return placedTowers[i].upgradesPurchased;
            }
        }
        return 0;
    }

    public string GetNameOfTowerClicked()
    {
        string nameOfTower = null;
        if (clickedTower.GetComponent<CannonTower>())
        {
            nameOfTower = "Cannon Tower";
        }
        else if (clickedTower.GetComponent<MissileTower>())
        {
            nameOfTower = "Missile Tower";
        }
        else if(clickedTower.GetComponent<SlowTower>())
        {
            nameOfTower = "Slow Tower";
        }
        else if(clickedTower.GetComponent<PoisonTower>())
        {
            nameOfTower = "Poison Tower";
        }
        return nameOfTower;
    }

    public PlacedTower GetPlacedTower(GameObject tower)
    {
        foreach (PlacedTower placedTower in placedTowers)
        {
            if (placedTower.tower.Equals(tower))
            {
                return placedTower;
            }
        }
        return null;
    }

    public void IncreaseUpgradesPurchased()
    {        
        for (int i = 0; i < placedTowers.Count; i++)
        {
            if (placedTowers[i].tower == clickedTower)
            {
                placedTowers[i].upgradesPurchased++;
            }
        }
    }
}
