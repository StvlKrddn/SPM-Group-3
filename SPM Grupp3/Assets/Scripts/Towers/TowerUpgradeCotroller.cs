using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradesPurchased
{
    public GameObject tower;
    public int upgradesPurchased;

    public UpgradesPurchased(GameObject t, int uP)
    {
        tower = t;
        upgradesPurchased = uP;
    }
}
public class TowerUpgradeCotroller : MonoBehaviour
{
    public static TowerUpgradeCotroller instance;
    public List<UpgradesPurchased> upgradeList = new List<UpgradesPurchased>();
    private GameObject clickedTower;

    public GameObject ClickedTower { get { return clickedTower; } set { clickedTower = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        EventHandler.Instance.RegisterListener<TowerClickedEvent>(GetTowerClicked);
    }

    public void GetTowerClicked(TowerClickedEvent eventInfo)
    {
        clickedTower = eventInfo.towerClicked;
    }

    public void PlaceTowerInUpgradeList(GameObject placedTower)
    {
        UpgradesPurchased U = new UpgradesPurchased(placedTower, 0);
        upgradeList.Add(U);
        
    }

    public int GetUpgradesPurchased()
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            if (upgradeList[i].tower.Equals(clickedTower))
            {
                return upgradeList[i].upgradesPurchased;
            }
        }
        return 0;
    }
    public void IncreaseUpgradesPurchased()
    {        
        for (int i = 0; i < upgradeList.Count; i++)
        {
            if (upgradeList[i].tower == clickedTower)
            {
                upgradeList[i].upgradesPurchased++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
