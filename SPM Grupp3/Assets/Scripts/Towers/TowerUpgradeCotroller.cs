using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradesPurchased
{
    public Tower tower;
    public int upgradesPurchased;
    public GameObject upgradeUIGameObject;

    public UpgradesPurchased(Tower t, int uP, GameObject uUIGO)
    {
        tower = t;
        upgradesPurchased = uP;
        upgradeUIGameObject = uUIGO;
    }
}
public class TowerUpgradeCotroller : MonoBehaviour
{
    public static TowerUpgradeCotroller instance;
    public Tower[] towerTypes;
    public List<UpgradesPurchased> upgradeList = new List<UpgradesPurchased>();
    public GameObject[] upgradeUIs;
    private GameObject uiGameobject;
    [SerializeField] private GameObject buildMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        for (int i = 0; i < towerTypes.Length; i++)
        {
            UpgradesPurchased U = new UpgradesPurchased(towerTypes[i], 0, upgradeUIs[i]);
            upgradeList.Add(U);
        }           
    }

    public GameObject GetUpgradesUI(Tower t)
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            if (upgradeList[i].tower == t)
            {
                print(upgradeList[i].upgradeUIGameObject);
                return upgradeList[i].upgradeUIGameObject;
            }
        }
        return null;
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
            }
        }
    }

    public void ShowUpgradeUI(Tower t)
    {
        if (uiGameobject != null)
        {
            uiGameobject.SetActive(false);
        }
        uiGameobject = GetUpgradesUI(t);

        uiGameobject.SetActive(true);
        buildMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
