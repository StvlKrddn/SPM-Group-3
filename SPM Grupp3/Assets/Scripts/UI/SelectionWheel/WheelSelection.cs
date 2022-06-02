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

    private UpgradeController upgradeController;
    private TowerManager towerManager;
    private GameManager gM;
    private GameObject towerToDisplay;
    private GameObject statisticPanel;
    private GameObject costPanel;
    private Text upgradeTitleText;
    private Text upgradeLevel1Text;
    private Text upgradeLevel2Text;
    private Text upgradeLevel3Text;

    private GameObject p1StatPanel;
    private GameObject p2StatPanel;

    private Text tankUpgradeTitleP1;
    private Text tankUpgradeTitleP2;

    private Text p1Damage;
    private Text p1FireRate;
    private Text p1Range;
    private Text p1Penetration;
    private Text p1Ability;


    private Text p2Damage;
    private Text p2FireRate;
    private Text p2Range;
    private Text p2Ability;

    private void Start() 
    {
        upgradeController = UpgradeController.Instance;
        towerManager = TowerManager.Instance;
        gM = GameManager.Instance;
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


        if (!gameObject.name.Equals("BuildPanel") && !gameObject.name.Equals("TankPanel"))
        {
            statisticPanel = MenuItems[0].transform.Find("StatisticPanel").gameObject;
            upgradeTitleText = statisticPanel.transform.Find("Title").GetComponent<Text>();
            upgradeLevel1Text = upgradeTitleText.transform.Find("Level1").GetComponent<Text>();
            upgradeLevel2Text = upgradeTitleText.transform.Find("Level2").GetComponent<Text>();
            upgradeLevel3Text = upgradeTitleText.transform.Find("Level3").GetComponent<Text>();

            SetStartValuesOnUpgrades();
        }  
        else if (gameObject.name.Equals("TankPanel"))
        {
            FindTankUpgradePanels();
        }
    }


    void FindTankUpgradePanels()
    {
        p1StatPanel = MenuItems[0].transform.Find("TankP1StatisticPanel").gameObject;
        p2StatPanel = MenuItems[0].transform.Find("TankP2StatisticPanel").gameObject;


        tankUpgradeTitleP1 = p1StatPanel.transform.Find("Title").GetComponent<Text>();
        p1Damage = tankUpgradeTitleP1.transform.Find("Damage").GetComponent<Text>();
        p1FireRate = tankUpgradeTitleP1.transform.Find("FireRate").GetComponent<Text>();
        p1Range = tankUpgradeTitleP1.transform.Find("Range").GetComponent<Text>();
        p1Penetration = tankUpgradeTitleP1.transform.Find("Penetration").GetComponent<Text>();
        p1Ability = tankUpgradeTitleP1.transform.Find("Ability").GetComponent<Text>();

        tankUpgradeTitleP2 = p2StatPanel.transform.Find("Title").GetComponent<Text>();
        p2Damage = tankUpgradeTitleP2.transform.Find("Damage").GetComponent<Text>();
        p2FireRate = tankUpgradeTitleP2.transform.Find("FireRate").GetComponent<Text>();
        p2Range = tankUpgradeTitleP2.transform.Find("Range").GetComponent<Text>();
        p2Ability = tankUpgradeTitleP2.transform.Find("Ability").GetComponent<Text>();
    }

    /* Sets the correct values on the statistics of the towers for statistic panel when upgradeing */
    private void SetStartValuesOnUpgrades()
    {
        switch (towerManager.GetNameOfTowerClicked())
        {
            case "Cannon Tower":
                CannonTower cannonTower = towerManager.ClickedTower.GetComponent<CannonTower>();
                upgradeLevel1Text.text = "Basic Fire rate: " + cannonTower.FireRate;
                upgradeLevel2Text.text = "Basic Damage: " + cannonTower.ShotDamage;
                upgradeLevel3Text.text = "Double Shot: (Inactive)";

                break;
            case "Missile Tower":
                MissileTower missileTower = towerManager.ClickedTower.GetComponent<MissileTower>();
                upgradeLevel1Text.text = "Basic AoE Radius: " + missileTower.SplashRadius;
                upgradeLevel2Text.text = "Basic AoE Damage: " + missileTower.SplashDamage;
                upgradeLevel3Text.text = "Increased Third Missle Dmg (Inactive)";

                break;
            case "Slow Tower":
                SlowTower sT = towerManager.ClickedTower.GetComponent<SlowTower>();
                upgradeLevel1Text.text = "Basic Range: " + sT.range;
                upgradeLevel2Text.text = "Basic Area Effect: (Active)";
                upgradeLevel3Text.text = "Periodic Halts All Enemies: (Inactive)";

                break;
            case "Poison Tower":
                PoisonTower poisonTower = towerManager.ClickedTower.GetComponent<PoisonTower>();
                upgradeLevel1Text.text = "Basic Poison Duration: " + poisonTower.PoisonTicks;
                upgradeLevel2Text.text = "Basic DoT: " + poisonTower.PoisonDamagePerTick;
                upgradeLevel3Text.text = "Contagious: (Inactive)";

                break;
        }        
    }

    private void Update() 
    {
        selectHint.SetActive(false);
        infoHint.SetActive(false);

        builderController.DestroyPreTower();

        if (!gameObject.name.Equals("BuildPanel") && !gameObject.name.Equals("TankPanel"))
        {
            upgradeLevel1Text.color = Color.white;
            upgradeLevel2Text.color = Color.white;
            upgradeLevel3Text.color = Color.white;
        }
        if (gameObject.name.Equals("TankPanel"))
        {
            p1Range.color = Color.white;
            p1Penetration.color = Color.white;
            p1FireRate.color = Color.white;
            p1Damage.color = Color.white;
            p1Ability.color = Color.white;

            p2Ability.color = Color.white;
            p2Damage.color = Color.white;
            p2FireRate.color = Color.white;
            //p2Range.color = Color.white;
        }


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
            if (MenuItems[i].transform.Find("Cost"))
            {
                costPanel = MenuItems[i].transform.Find("Cost").gameObject;
            }

            if (gameObject.name.Equals("BuildPanel"))
            {
                statisticPanel = MenuItems[i].transform.Find("StatisticPanel").gameObject;
            }

            if (i == index)
            {
                //print(i + " - " + index);
                // Hover effect
                MenuItems[i].transform.GetChild(0).GetComponent<Image>().color = highLight;
                    
                costPanel.SetActive(true);
                Text moneyText = costPanel.transform.Find("MoneyCost").GetComponentInChildren<Text>();
                Text materialText = costPanel.transform.Find("MaterialCost").GetComponentInChildren<Text>();

                Tower tower;

                DecideTowerToBuild(MenuItems[i].name);
                if (towerToDisplay != null)
                {
                    tower = towerToDisplay.GetComponent<Tower>();
                    moneyText.text = tower.cost.ToString();
                    materialText.text = tower.materialCost.ToString(); //if needed

                    if (gM.CheckIfEnoughResources(towerToDisplay.GetComponent<Tower>()))
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
                    if (towerManager.ClickedTower != null) //Checks upgrade and changes the UI based on upgradelevel
                    {
                        UpgradeHighlighted(moneyText, materialText);
                    }
                    else if (gameObject.name.Equals("TankPanel"))
                    {
                        GarageHighlight(materialText);
                    }
                }
                
                if (!MenuItems[i].name.Equals("Delete/Sell"))
                {
                    ShowStatisticPanel();
                }
                
                if (MenuItems[0].name.Equals("Upgrade") && !gameObject.name.Equals("TankPanel"))
                {
                    if (stickAction.ReadValue<Vector2>().y < 0.4f)
                    {                     
                        statisticPanel.SetActive(false);
                    }
                }
                else if(gameObject.name.Equals("TankPanel"))
                {
                    if (stickAction.ReadValue<Vector2>().y < 0.4f)
                    {
                        p1StatPanel.SetActive(false);
                        p2StatPanel.SetActive(false);
                    }                   
                }

                selectHint.SetActive(true);
                infoHint.SetActive(true);               
            }
            else
            {
                // Remove hover effect from all other items
                MenuItems[i].transform.Find("Background").GetComponent<Image>().color = hidden;

                MenuItems[i].transform.Find("Cost").gameObject.SetActive(false);


                if (gameObject.name.Equals("BuildPanel"))
                {
                    statisticPanel.SetActive(false);
                }

            }
        }
    }

    // Decides which statistic panel to show
    private void ShowStatisticPanel()
    {

        if (transform.name.Equals("BuildPanel")) //Build
        {
            if (infoAction.IsPressed())
            {
                statisticPanel.SetActive(true);
            }
        }
        else if (gameObject.name.Equals("TankPanel")) //Tank Upgrade (Not Implemented Jet)
        {
            if (infoAction.IsPressed())
            {
                p1StatPanel.SetActive(true);
                p2StatPanel.SetActive(true);               
            }
            ChangeTankUpgradeTextColorToGreen();
        }
        else //Tower Upgrade
        {
            if (infoAction.IsPressed())
            {
                statisticPanel.SetActive(true);
            }
            upgradeTitleText.text = towerManager.GetNameOfTowerClicked() + " (Lv " + towerManager.GetUpgradesPurchased() + ")";
            ChangeUpgradeTextColorToGreen();
        }
        
    }

    private void ChangeTankUpgradeTextColorToGreen()
    {
        switch (upgradeController.GetUpgradesPurchased())
        {
            case 0:
                p1Damage.color = Color.green;
                p1FireRate.color = Color.green;
                p1Range.color = Color.green;

                p2FireRate.color = Color.green;

                tankUpgradeTitleP1.text = "Player 1 Tank (LVL 1)";
                tankUpgradeTitleP2.text = "Player 2 Tank (LVL 1)";
                break;
            case 1:
                p1Range.color = Color.green;
                p1Penetration.color = Color.green;

                p2Damage.color = Color.green;
                p2FireRate.color = Color.green;

                tankUpgradeTitleP1.text = "Player 1 Tank (LVL 2)";
                tankUpgradeTitleP2.text = "Player 2 Tank (LVL 2)";
                break;
            case 2:
                p1Ability.color = Color.green;
                p2Ability.color = Color.green;

                tankUpgradeTitleP1.text = "Player 1 Tank (LVL 3)";
                tankUpgradeTitleP2.text = "Player 2 Tank (LVL 3)";
                break;
            case 3:
                tankUpgradeTitleP1.text = "Player 1 Tank (LVL Max)";
                tankUpgradeTitleP2.text = "Player 2 Tank (LVL Max)";
                break;
        }

    }

    private void ChangeUpgradeTextColorToGreen()
    {
        switch (towerManager.GetUpgradesPurchased()) //Which level the tower is in and which text to turn green
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
                upgradeTitleText.text = towerManager.GetNameOfTowerClicked() + " (Lv MAX)";
                break;
        }
    }

    /* Method for determining how the cost will be presented to the player */
    private void UpgradeHighlighted(Text moneyText, Text materialText)
    {
        Tower tower;
        tower = towerManager.ClickedTower.GetComponent<Tower>();

        if (towerManager.GetUpgradesPurchased() == 3)
        {
            moneyText.text = "MAX";
            materialText.text = "MAX";

            moneyText.color = Color.red;
            materialText.color = Color.red;
            return;
        }
        else if (towerManager.GetUpgradesPurchased() == 2)
        {
            GameObject materialPanel = costPanel.transform.GetChild(1).gameObject;
            materialPanel.SetActive(true);
            materialText = materialPanel.GetComponentInChildren<Text>();

            materialText.text = tower.Level3MaterialCost.ToString();
        }
        
        float money = gM.Money;
        float material = gM.Material;

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
        //materialText.text = tower.materialCost.ToString();
    }

    void GarageHighlight(Text materialText)
    {
        float material = gM.Material;

        if (upgradeController.GetUpgradesPurchased() == 3)
        {
            materialText.text = "MAX";
            materialText.color = Color.red;
            return;
        }

        if (material < upgradeController.materialCost)
        {
            materialText.color = Color.red;
        }
        else
        {
            materialText.color = Color.black;
        }
    }

    public void DecideTowerToBuild(string name)
    {
        switch (name) // Which tower that is selected
        {
            case "Cannon":
                towerToDisplay = buildManager.CannonTowerPrefab;
                break;

            case "Missile":
                towerToDisplay = buildManager.MissileTowerPrefab;
                break;

            case "Slow":
                towerToDisplay = buildManager.SlowTowerPrefab;
                break;

            case "Poison":
                towerToDisplay = buildManager.PoisonTowerPrefab;
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

            if (!gameObject.name.Equals("BuildPanel") && !gameObject.name.Equals("TankPanel"))
            {
                UpdateUpgradeLevelText();                              
            }

            else if (gameObject.name.Equals("TankPanel"))
            {
                UpdateTankUpgradeLevelText();
            }
            stickInput = Vector2.zero;
        }
        else
        {
                /*MenuItems[index].color = Color.green;*/
        }
    }

    private void UpdateTankUpgradeLevelText()
    {
        switch (upgradeController.GetUpgradesPurchased())
        {
            case 1:
                p1Damage.text = "Damage: 90";
                p1FireRate.text = "FireRate: 0.75";
                p1Range.text = "Range: 25";

                p2FireRate.text = "FireRate: 8";

                break;
            case 2:
                p1Range.text = "Range: 100";
                p1Penetration.text = "Penetration: 4";

                p2Damage.text = "Damage: 7.5/s";
                p2FireRate.text = "FireRate: 100";
                break;
            case 3:
                p1Ability.text = "Damage To The Enemy Type (Active)";
                p2Ability.text = "Drops Bomb (Active)";

                break;
        }
    }

    private void UpdateUpgradeLevelText()
    {
        switch (towerManager.GetNameOfTowerClicked()) //Which Tower That Is Clicked
        {
            case "Cannon Tower":
                CannonTower cannonTower = towerManager.ClickedTower.GetComponent<CannonTower>();

                switch (towerManager.GetUpgradesPurchased()) // Which Level the CannonTower is in
                {
                    case 1:
                        upgradeLevel1Text.text = "Basic Fire rate: " + cannonTower.FireRate;
                        break;
                    case 2:
                        upgradeLevel2Text.text = "Basic Damage: " + cannonTower.ShotDamage;
                        break;
                    case 3:
                        upgradeLevel3Text.text = "Double Shot: (Active)";
                        break;
                }

                break;
            case "Missile Tower":
                MissileTower missileTower = towerManager.ClickedTower.GetComponent<MissileTower>();

                switch (towerManager.GetUpgradesPurchased()) // Which Level the MissileTower is in
                {
                    case 1:
                        upgradeLevel1Text.text = "Basic AoE Radius: " + missileTower.SplashRadius;
                        break;
                    case 2:
                        upgradeLevel2Text.text = "Basic AoE Damage: " + missileTower.SplashDamage;
                        break;
                    case 3:
                        upgradeLevel3Text.text = "Increased Third Missle Dmg (Active)";
                        break;
                }
                break;
            case "Slow Tower":
                SlowTower slowTower = towerManager.ClickedTower.GetComponent<SlowTower>();

                switch (towerManager.GetUpgradesPurchased()) // Which Level the SlowTower is in
                {
                    case 1:
                        upgradeLevel1Text.text = "Basic Range: " + slowTower.range;
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
                PoisonTower poisonTower = towerManager.ClickedTower.GetComponent<PoisonTower>();

                switch (towerManager.GetUpgradesPurchased()) // Which Level the PoisonTower is in
                {
                    case 1:
                        upgradeLevel1Text.text = "Basic Poison Duration: " + poisonTower.PoisonTicks;
                        break;
                    case 2:
                        upgradeLevel2Text.text = "Basic DoT: " + poisonTower.PoisonDamagePerTick;
                        break;
                    case 3:
                        upgradeLevel3Text.text = "Contagious: (Active)";
                        break;
                }
                break;
        }
    }
}
