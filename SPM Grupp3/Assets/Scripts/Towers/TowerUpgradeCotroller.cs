using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradesPurchased
{
    public Tower tower;
    public int upgradesPurchased;

    public UpgradesPurchased(Tower t, int uP)
    {
        tower = t;
        upgradesPurchased = uP;
    }
}
public class TowerUpgradeCotroller : MonoBehaviour
{
    public static TowerUpgradeCotroller instance;
    public List<UpgradesPurchased> upgradeList = new List<UpgradesPurchased>();


    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

/*        for (int i = 0; i < upgradeList.Count; i++)
        {
            UpgradesPurchased U = new UpgradesPurchased(towerTypes[i], 0);
            upgradeList.Add(U);
        } */          
    }

    public void PlaceTowerInUpgradeList(Tower t)
    {
        UpgradesPurchased U = new UpgradesPurchased(t, 0);
        upgradeList.Add(U);
    }

    public int GetUpgradesPurchased(Tower t)
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            if (upgradeList[i].tower == t)
            {
                return upgradeList[i].upgradesPurchased;
            }
        }
        return 0;
    }
    public void IncreaseUpgradesPurchased(Tower t)
    {       
        for (int i = 0; i < upgradeList.Count; i++)
        {
            if (upgradeList[i].tower == t)
            {
                upgradeList[i].upgradesPurchased++;
                print(upgradeList[i].upgradesPurchased);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
