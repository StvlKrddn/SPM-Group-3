using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeController : MonoBehaviour
{
    public static UpgradeController instance;
    public int currentUpgradeLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        //EventHandler.Instance.RegisterListener<TowerClickedEvent>(GetTowerClicked);
    }

    public int GetUpgradesPurchased()
    {
        return currentUpgradeLevel;
    }

    public void IncreaseUpgradesPurchased()
    {
        currentUpgradeLevel++;
        if(FindObjectOfType<TankState>())
        {
            TankState player = FindObjectOfType<TankState>();
            print(player.gameObject.transform.parent.name);
            switch (currentUpgradeLevel)
            {
                case 1:
                player.tankUpgradeTree.UpgradeOne();
                break;

                case 2:
                player.tankUpgradeTree.UpgradeTwo();
                break;

                case 3:
                player.tankUpgradeTree.UpgradeThree();
                break;
            }
        }
    }

    public void FixUpgrades(GameObject player)
    {
        TankState tS = player.GetComponentInChildren<TankState>();
        for (int i = 1; i <= currentUpgradeLevel; i++)
        {
            switch (i)
            {
                case 1:
                    tS.tankUpgradeTree.UpgradeOne();
                    break;

                case 2:
                    tS.tankUpgradeTree.UpgradeTwo();
                    break;

                case 3:
                    tS.tankUpgradeTree.UpgradeThree();
                    break;
            }
        }
    }
}
