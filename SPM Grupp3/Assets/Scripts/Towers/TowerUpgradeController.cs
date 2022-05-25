using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacedTower
{
    public GameObject tower;
    public int upgradesPurchased;

    public PlacedTower(GameObject t, int uP)
    {
        tower = t;
        upgradesPurchased = uP;
    }
}
public class TowerUpgradeController : MonoBehaviour
{
    private static TowerUpgradeController instance;
    private GameObject clickedTower;
    private GameManager gameManager;
    private List<PlacedTower> placedTowers;

    public GameObject ClickedTower { get { return clickedTower; } set { clickedTower = value; } }
    public static TowerUpgradeController Instance 
    { 
        get 
        {
            // "Lazy loading" to prevent Unity load order error
            if (instance == null)
            {
                instance = FindObjectOfType<TowerUpgradeController>();
            }
            return instance; 
        } 
    }

    void Awake()
    {
        gameManager = GameManager.Instance;
        placedTowers = gameManager.towersPlaced;

        EventHandler.Instance.RegisterListener<TowerClickedEvent>(GetTowerClicked);
    }

    public void GetTowerClicked(TowerClickedEvent eventInfo)
    {
        clickedTower = eventInfo.towerClicked;
    }

    public void PlaceTowerInUpgradeList(GameObject placedTower)
    {
        PlacedTower U = new PlacedTower(placedTower, 0);
        gameManager.AddPlacedTower(U);   
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
