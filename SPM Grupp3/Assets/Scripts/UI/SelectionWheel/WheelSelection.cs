using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/* Based on: https://answers.unity.com/questions/1592985/how-would-i-create-an-item-wheel-using-analog-stic.html*/
public class WheelSelection : MonoBehaviour
{
    public PlayerInput playerInput;

    // Put in order from right going counter-clockwise
    public GameObject[] MenuItems;

    // Is the first item centered on the X-axis?
    public bool FirstCenteredX = true;

    // Smallest degree that measure the first item. Should be 0 or lower
    private float firstItemDegrees = 0;

    // Current direction the player is pointing, -1 is no input
    [SerializeField] private Color highLight;
    [SerializeField] private Color hidden;

    [SerializeField] private BuilderController builderController;
    [SerializeField] private BuildManager buildManager;
    [SerializeField] private GameObject selectHint;
    [SerializeField] private GameObject infoHint;

    private float pointingAngle;
    private float degreesPerItem;
    private float overflowFirstItem;
    private float numberOfMenuItems;
    private int selectedIndex = -1;

    private InputAction stickAction;
    private InputAction selectAction;
    private Vector2 stickInput;
    private InputAction infoAction;

    private TowerUpgradeController tUC;
    private GameObject towerToDisplay;
    private GameObject statisticPanel;
    private GameObject menuItem;
    private Text upgradeTitleText;
    private Text upgradeLevel1Text;
    private Text upgradeLevel2Text;
    private Text upgradeLevel3Text;


    private void Start() 
    {
        tUC = TowerUpgradeController.Instance;
        numberOfMenuItems = MenuItems.Length;
        degreesPerItem = 360 / numberOfMenuItems;

        if (FirstCenteredX)
        {
            firstItemDegrees = 0 - degreesPerItem / 2;
            overflowFirstItem = 360 - degreesPerItem / 2;
        }
        else 
        {
            overflowFirstItem = 0;
            firstItemDegrees = 0;
        }

        playerInput.SwitchCurrentActionMap("Builder");
        stickAction = playerInput.actions["LeftStick"];
        selectAction = playerInput.actions["Accept"];
        infoAction = playerInput.actions["Information"];

        SetRightValuesOnUpgrades();
    }

    private void SetRightValuesOnUpgrades()
    {
        if (gameObject.name.Equals("BuildPanel"))
        {
            return;
        }
        if (gameObject.name.Equals("TankPanel"))
        {
            return;
        }
        
        statisticPanel = MenuItems[0].transform.Find("StatisticPanel").gameObject;
        upgradeTitleText = statisticPanel.transform.Find("Title").GetComponent<Text>();
        upgradeLevel1Text = upgradeTitleText.transform.Find("Level1").GetComponent<Text>();
        upgradeLevel2Text = upgradeTitleText.transform.Find("Level2").GetComponent<Text>();
        upgradeLevel3Text = upgradeTitleText.transform.Find("Level3").GetComponent<Text>();

        switch (tUC.GetNameOfTowerClicked())
        {
            case "Cannon Tower":
                CannonTower cT = tUC.ClickedTower.GetComponent<CannonTower>();
                upgradeLevel1Text.text = "Basic Fire rate: " + cT.FireRate;
                upgradeLevel2Text.text = "Basic Damage: " + cT.ShotDamage;
                upgradeLevel3Text.text = "Double Shot: (Active)";

                break;
            case "Missile Tower":
                MissileTower mT = tUC.ClickedTower.GetComponent<MissileTower>();
                upgradeLevel1Text.text = "Basic AoE Radius: " + mT.SplashRadius;
                upgradeLevel2Text.text = "Basic AoE Damage: " + mT.SplashDamage;
                upgradeLevel3Text.text = "Increased Third Missle Dmg (Active)";

                break;
            case "Slow Tower":
                SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
                upgradeLevel1Text.text = "Basic Range: " + sT.range;
                upgradeLevel2Text.text = "Basic Area Effect: (Active)";
                upgradeLevel3Text.text = "Periodic Halts All Enemies: (Active)";

                break;
            case "Poison Tower":
                PoisonTower pT = tUC.ClickedTower.GetComponent<PoisonTower>();
                upgradeLevel1Text.text = "Basic Poison Duration: " + pT.PoisonTicks;
                upgradeLevel2Text.text = "Basic DoT: " + pT.PoisonDamagePerTick;
                upgradeLevel3Text.text = "Contagious: (Active)";

                break;
        }
        
    }

    private void Update() 
    {
        selectHint.SetActive(false);
        infoHint.SetActive(false);

        builderController.DestroyPreTower();

        stickInput = stickAction.ReadValue<Vector2>();

        if (!builderController.purchasedTower)
        {
            if (stickAction.IsPressed())
            {
                float signedAngle = Vector2.SignedAngle(Vector2.up, stickInput);
                if (signedAngle <= 0)
                {
                    signedAngle += 360;
                }

                // Get angle of stick
                pointingAngle = signedAngle;

                selectedIndex = GetIndex(pointingAngle);
            }
            else
            {
                selectedIndex = -1;
            }

            HighlightItem(selectedIndex);

            if (selectAction.triggered && selectedIndex != -1)
            {
                SelectItem(selectedIndex);
            }
        }
    }

    // Returns index of item based on angle
    private int GetIndex(float angle)
    {
        // Check if there are any menu items
        if (numberOfMenuItems <= 0 || angle < 0)
        {
            return -1;
        }

        // Check if pointing at the first item because it can cross axis

        if (angle < firstItemDegrees + degreesPerItem || angle > overflowFirstItem)
        {
            return 0;
        }

        int itemIndex = -1;

        for (int i = 1; i <= numberOfMenuItems; i++)
        {
            if (angle > i * degreesPerItem + firstItemDegrees && angle < i * (degreesPerItem + degreesPerItem) + firstItemDegrees)
            {
                itemIndex = i;
            }
        }

        return itemIndex;
    }

    void HighlightItem(int index)
    {
        if (numberOfMenuItems <= 0)
        {
            return;
        }
        for (int i = 0; i < numberOfMenuItems; i++)
        {            
            if (MenuItems[i].transform.Find("StatisticPanel"))
            {
                statisticPanel = MenuItems[i].transform.Find("StatisticPanel").gameObject;

                if (!MenuItems[i].transform.parent.name.Equals("BuildPanel"))
                {
                    upgradeTitleText = statisticPanel.transform.Find("Title").GetComponent<Text>();
                    upgradeLevel1Text = upgradeTitleText.transform.Find("Level1").GetComponent<Text>();
                    upgradeLevel2Text = upgradeTitleText.transform.Find("Level2").GetComponent<Text>();
                    upgradeLevel3Text = upgradeTitleText.transform.Find("Level3").GetComponent<Text>();
                }
            }

            if (MenuItems[i].transform.Find("Cost"))
            {
                menuItem = MenuItems[i].transform.Find("Cost").gameObject;
            }

            if (i == index)
            {
                // Hover effect
                MenuItems[i].transform.GetChild(0).GetComponent<Image>().color = highLight;
                    
                menuItem.SetActive(true);
                Text moneyText = menuItem.transform.Find("MoneyCost").GetComponentInChildren<Text>();
                Text materialText = menuItem.transform.Find("MaterialCost").GetComponentInChildren<Text>();
                    
                Tower tower;

                ShowStatisticPanel(i);

                DecideTowerToBuild(MenuItems[i].name);

                if (towerToDisplay != null)
                {
                    tower = towerToDisplay.GetComponent<Tower>();
                    moneyText.text = tower.cost.ToString();
                    materialText.text = tower.materialCost.ToString(); //if needed

                    if (GameManager.Instance.CheckIfEnoughResources(towerToDisplay.GetComponent<Tower>()))
                    {
                        moneyText.color = Color.black;
                        builderController.GhostTower(towerToDisplay);
                    }
                    else
                    {
                        moneyText.color = Color.red;
                    }
                }
                else
                {                  
                    if (tUC.ClickedTower != null) //Checks upgrade and changes the UI based on upgradelevel
                    {
                        UpgradeHighlighted(moneyText, materialText);
                    }
                }
/*
                DecideTowerToBuild(MenuItems[i].name);
                if (towerToDisplay != null)
                {

                }
                else
                {
                    
                    if (tUC.ClickedTower != null) //Checks upgrade and changes the UI based on upgradelevel
                    {
                        UpgradeHighlighted(moneyText, materialText, tUC);
                    }
                }

                if (infoAction.triggered)
                {
                    //InfoPanel();
                }*/

                selectHint.SetActive(true);
                infoHint.SetActive(true);               
            }
            else
            {
                // Remove hover effect from all other items
                MenuItems[i].transform.GetChild(0).GetComponent<Image>().color = hidden;

                MenuItems[i].transform.Find("Cost").gameObject.SetActive(false);

                if (MenuItems[i].transform.parent.name.Equals("BuildPanel"))
                {
                    statisticPanel.SetActive(false);                    
                }
                else
                {
                    upgradeLevel1Text.color = Color.white;
                    upgradeLevel2Text.color = Color.white;
                    upgradeLevel3Text.color = Color.white;
                }
            }
        }
    }

    private void ShowStatisticPanel(int i)
    {
        if (statisticPanel != null)
        {
            if (MenuItems[i].transform.parent.name.Equals("BuildPanel"))
            {
                if (infoAction.IsPressed())
                {
                    statisticPanel.SetActive(true);
                }
            }
            else
            {
                statisticPanel.SetActive(true);

                upgradeTitleText.text = tUC.GetNameOfTowerClicked() + " (Lv " + tUC.GetUpgradesPurchased() + ")";

                ChangeUpgradeTextColorToGreen();
            }
        }
    }

    private void ChangeUpgradeTextColorToGreen()
    {
        switch (tUC.GetUpgradesPurchased())
        {
            case 0:
                upgradeLevel1Text.color = Color.green;
                break;
            case 1:
                upgradeLevel2Text.color = Color.green;
                break;
            case 2:
                upgradeLevel3Text.color = Color.green;
                break;
            case 3:
                upgradeTitleText.text = tUC.GetNameOfTowerClicked() + " (Lv MAX)";
                break;
        }
    }

    private void UpgradeHighlighted(Text moneyText, Text materialText)
    {
        Tower tower;
        tower = tUC.ClickedTower.GetComponent<Tower>();
        if (tUC.GetUpgradesPurchased() == 3)
        {
            moneyText.text = "MAX";
            materialText.text = "MAX";

            moneyText.color = Color.red;
            materialText.color = Color.red;
            return;
        }
        
        float money = GameManager.Instance.Money;
        float material = GameManager.Instance.Material;

        if(money < tower.UpgradeCostUpdate())
        {
            moneyText.color = Color.red;
        }
        else
        {
            moneyText.color = Color.black;
        }

        if(material < tower.materialCost)
        {
            materialText.color = Color.red;
        }
        else
        {
            materialText.color = Color.black;
        }

        moneyText.text = tower.UpgradeCostUpdate().ToString();
        materialText.text = tower.materialCost.ToString();
    }

    public void DecideTowerToBuild(string name)
    {
        switch (name)
        {
            case "Cannon":
                towerToDisplay = buildManager.cannonTowerPrefab;
                break;

            case "Missile":
                towerToDisplay = buildManager.missileTowerPrefab;
                break;

            case "Slow":
                towerToDisplay = buildManager.slowTowerPrefab;
                break;

            case "Poison":
                towerToDisplay = buildManager.poisonTowerPrefab;
                break;

            default:
                towerToDisplay = null;
                break;
         }
    }

    void SelectItem(int index)
    {
        bool isPressed = selectAction.triggered;
        GameObject selectedItem = MenuItems[index].gameObject;
        if (isPressed)
        {
            selectedItem.GetComponent<ButtonClick>().Click();
            EventHandler.InvokeEvent(new BoughtInUIEvent("Something is bought in UI"));

            // if (!MenuItems[index].transform.parent.name.Equals("BuildPanel"))
            // {
            //     UpdateUpgradeLevelText();
            // }

            stickInput = Vector2.zero;
        }
        else
        {
                /*MenuItems[index].color = Color.green;*/
        }
    }


    private void UpdateUpgradeLevelText()
    {
        switch (tUC.GetNameOfTowerClicked())
        {
            case "Cannon Tower":
                CannonTower cT = tUC.ClickedTower.GetComponent<CannonTower>();
                switch (tUC.GetUpgradesPurchased())
                {
                    case 1:
                        upgradeLevel1Text.text = "Basic Fire rate: " + cT.FireRate;
                        break;
                    case 2:
                        upgradeLevel2Text.text = "Basic Damage: " + cT.ShotDamage;
                        break;
                    case 3:
                        upgradeLevel3Text.text = "Double Shot: (Active)";
                        break;
                }

                break;
            case "Missile Tower":
                MissileTower mT = tUC.ClickedTower.GetComponent<MissileTower>();
                switch (tUC.GetUpgradesPurchased())
                {
                    case 1:
                        upgradeLevel1Text.text = "Basic AoE Radius: " + mT.SplashRadius;
                        break;
                    case 2:
                        upgradeLevel2Text.text = "Basic AoE Damage: " + mT.SplashDamage;
                        break;
                    case 3:
                        upgradeLevel3Text.text = "Increased Third Missle Dmg (Active)";
                        break;
                }
                break;
            case "Slow Tower":
                SlowTower sT = tUC.ClickedTower.GetComponent<SlowTower>();
                switch (tUC.GetUpgradesPurchased())
                {
                    case 1:
                        upgradeLevel1Text.text = "Basic Range: " + sT.range;
                        break;
                    case 2:
                        upgradeLevel2Text.text = "Basic Area Effect: (Active)";
                        break;
                    case 3:
                        upgradeLevel3Text.text = "Periodic Halts All Enemies: (Active)";
                        break;
                }
                break;
            case "Poison Tower":
                PoisonTower pT = tUC.ClickedTower.GetComponent<PoisonTower>();
                switch (tUC.GetUpgradesPurchased())
                {
                    case 1:
                        upgradeLevel1Text.text = "Basic Poison Duration: " + pT.PoisonTicks;
                        break;
                    case 2:
                        upgradeLevel2Text.text = "Basic DoT: " + pT.PoisonDamagePerTick;
                        break;
                    case 3:
                        upgradeLevel3Text.text = "Contagious: (Active)";
                        break;
                }
                break;

        }
    }
}
