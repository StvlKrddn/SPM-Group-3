using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeController : MonoBehaviour
{
    private static UpgradeController instance;
    [SerializeField] private int moneyCost = 500;
    public int materialCost = 0;

    public static int currentUpgradeLevel = 0;

    public static UpgradeController Instance
    {
        get
        {
            // "Lazy loading" to prevent Unity load order error
            if (instance == null)
            {
                instance = FindObjectOfType<UpgradeController>();
            }
            return instance;
        }
    }

    public int GetUpgradesPurchased()
    {
        return currentUpgradeLevel;
    }

	public void IncreaseUpgradesPurchased()
    {
        if (currentUpgradeLevel < 3 && GameManager.Instance.SpendResources(moneyCost, materialCost))
        {
            currentUpgradeLevel++;
            if(FindObjectOfType<TankState>())
            {
                TankState player = FindObjectOfType<TankState>();
                player.LevelOfTank++;
                switch (player.LevelOfTank)
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

                if (player.GetComponent<WeaponSlot>())
                {
                    player.GetComponent<WeaponSlot>().UpgradeShots();
                }
            }
        }
    }

    public void FixUpgrades(GameObject player)
    {
        TankState tS = player.GetComponentInChildren<TankState>();
        for (int i = tS.LevelOfTank; i < currentUpgradeLevel; i++)
        {
            tS.LevelOfTank++;
            switch (tS.LevelOfTank)
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
        if (tS.GetComponent<WeaponSlot>())
        {
            tS.GetComponent<WeaponSlot>().UpgradeShots();
        }
    }
}
