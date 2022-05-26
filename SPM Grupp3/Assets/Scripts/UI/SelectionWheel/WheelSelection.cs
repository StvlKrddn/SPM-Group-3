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

    private GameObject towerToDisplay;

    private void Start() 
    {
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
    }

    private void Update() 
    {
        selectHint.SetActive(false);
        infoHint.SetActive(false);

        builderController.DestroyPreTower();

        stickInput = stickAction.ReadValue<Vector2>();

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
            if (i == index)
            {
                // Hover effect
                MenuItems[i].transform.GetChild(0).GetComponent<Image>().color = highLight;
                if (MenuItems[i].transform.Find("Cost"))
                {
                    GameObject menuItem = MenuItems[i].transform.Find("Cost").gameObject;
                    menuItem.SetActive(true);
                    Text moneyText = menuItem.transform.Find("MoneyCost").GetComponentInChildren<Text>();
                    Text materialText = menuItem.transform.Find("MaterialCost").GetComponentInChildren<Text>();
                    Tower tower;

                    if (towerToDisplay != null)
                    {
                        tower = towerToDisplay.GetComponent<Tower>();
                        moneyText.text = tower.cost.ToString();
                        materialText.text = tower.materialCost.ToString(); //if needed
                    }
                    else
                    {
                        TowerUpgradeController tUC = TowerUpgradeController.Instance;
                        if (tUC.ClickedTower != null) //Checks upgrade and changes the UI based on upgradelevel
                        {
                            UpgradeHighlighted(moneyText, materialText, tUC);
                        }
                    }

                    DecideTowerToBuild(MenuItems[i].name);
                    if (towerToDisplay != null)
                    {
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

                    if (infoAction.triggered)
                    {
                        print("WEEEEEEEEEEEEEE");
                        //InfoPanel();
                    }


                    selectHint.SetActive(true);
                    infoHint.SetActive(true);
                }
            }
            else
            {
                // Remove hover effect from all other items
                MenuItems[i].transform.GetChild(0).GetComponent<Image>().color = hidden;
                if (MenuItems[i].transform.Find("Cost"))
                {
                    MenuItems[i].transform.Find("Cost").gameObject.SetActive(false);
                }

            }
        }
    }

    private void UpgradeHighlighted(Text moneyText, Text materialText, TowerUpgradeController tUC)
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

        if (GameManager.Instance.CheckIfEnoughResources(tower))
        {
             moneyText.color = Color.black;
             materialText.color = Color.black;
        }
        else if (money < tower.cost)
        {
            moneyText.color = Color.red;
            materialText.color = Color.black;
        }
        else if (material < tower.materialCost)
        {
            moneyText.color = Color.black;
            materialText.color = Color.red;   
        }
        else
        {
            moneyText.color = Color.red;
            materialText.color = Color.red;
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
         }
    }

    void SelectItem(int index)
    {
        bool isPressed = selectAction.triggered;
        GameObject selectedItem = MenuItems[index].gameObject;
        if (isPressed)
        {
            selectedItem.GetComponent<ButtonClick>().Click();
            EventHandler.Instance.InvokeEvent(new BoughtInUIEvent("Something is bought in UI"));
        }
        else
        {
                /*MenuItems[index].color = Color.green;*/
        }
    }
}
