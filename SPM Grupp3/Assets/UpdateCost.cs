using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCost : MonoBehaviour
{
    private GameObject cost;
    private Text costText;
    private Text materialText;
    private GameObject towerPanel;
    [SerializeField] private BuildManager buildManager;
    private GameObject tower;
    // Start is called before the first frame update
    void Start()
    {
        cost = transform.Find("Cost").gameObject;

        costText = cost.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        materialText = cost.transform.GetChild(1).GetChild(0).GetComponent<Text>();

        towerPanel = transform.parent.gameObject;
    }

    void TowerToCost()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (towerPanel.name)
        {
            case "UpgradeCannonPanel":
                tower = buildManager.cannonTowerPrefab;
                break;
            case "UpgradeMissilePanel":
                tower = buildManager.missileTowerPrefab;
                break;
            case "UpgradeSlowPanel":
                tower = buildManager.slowTowerPrefab;
                break;
            case "UpgradePoisonPanel":
                tower = buildManager.poisonTowerPrefab;
                break;

        }

        costText.text = tower.GetComponent<Tower>().UpgradeCostUpdate().ToString();

        /*        TowerUpgradeCotroller tUC = TowerUpgradeCotroller.instance;

                switch(tUC.GetUpgradesPurchased())
                {
                    case 0:
                        costText.text = tower.GetComponent<Tower>()..ToString();
                        materialText.text = tower.GetComponent<Tower>().materialCost.ToString();
                        break;
                    case 1:
                        costText.text = tower.GetComponent<Tower>().cost.ToString();
                        materialText.text = tower.GetComponent<Tower>().materialCost.ToString();
                        break;
                    case 2:
                        costText.text = tower.GetComponent<Tower>().cost.ToString();
                        materialText.text = tower.GetComponent<Tower>().materialCost.ToString();
                        break;
                }
            }*/
    }
}