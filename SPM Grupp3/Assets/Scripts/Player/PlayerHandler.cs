using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerHandler : MonoBehaviour
{

    [SerializeField] private GameObject tankMode;
    [SerializeField] private GameObject buildMode;
    [SerializeField] private GameObject cursor;

    private PlayerMode currentMode;
    private GameObject canvas;
    private PlayerInput playerInput;
    private bool destroyed;

    public bool Destroyed { get { return destroyed; } set { destroyed = value; } }
    public PlayerMode CurrentMode { get {return currentMode; } }

    void Start()
    {
        destroyed = false;

        canvas = UI.Canvas.gameObject;

        currentMode = GameManager.Instance.StartingMode;

        playerInput = GetComponent<PlayerInput>();

        playerInput.uiInputModule = GetComponent<InputSystemUIInputModule>();

        // First mode set up
        if (currentMode == PlayerMode.Build)
        {
            EnterBuildMode();
            
        }
        else if (currentMode == PlayerMode.Tank)
        {
            EnterTankMode();
        }
        //buildMenu.SetActive(currentMode == PlayerMode.Build);
    }

    void EnterTankMode()
    {
        // Disable Build
        buildMode.SetActive(false);
        cursor.SetActive(false);

        // Enable Tank
        tankMode.SetActive(true);

        EventHandler.InvokeEvent(new EnterTankModeEvent(
            description: "Player entered tank mode",
            playerContainer: gameObject
            ));

        playerInput.SwitchCurrentActionMap("Tank");

        currentMode = PlayerMode.Tank;


        StartCoroutine(FixUpgradeDelay(gameObject));
    }

    private IEnumerator FixUpgradeDelay(GameObject tank)
    {
        yield return new WaitForSeconds(0.01f);
        UpgradeController.Instance.FixUpgrades(tank);
    }

    void EnterBuildMode()
    {
        // Disable Tank
        tankMode.SetActive(false);

        // Enable Build
        buildMode.SetActive(true);
        cursor.SetActive(true);

        EventHandler.InvokeEvent(new EnterBuildModeEvent(
            description: "Player entered build mode",
            player: gameObject
            ));

        playerInput.SwitchCurrentActionMap("Builder");

        currentMode = PlayerMode.Build;

        //print("Entered build mode");
    }

    public void SwitchMode()
    {
        // If in Tank mode, switch to Build
        if (currentMode == PlayerMode.Tank)
        {
            EnterBuildMode();
        }
        // If in Build mode, switch to Tank
        else if (currentMode == PlayerMode.Build && !destroyed)
        {
            EnterTankMode();
        }
        if (destroyed)
        {
            //print("Your tank is destroyed! Wait until next wave");
        }
    }

    public void RepairTank()
    {
        if (GameManager.Instance.SpendResources(500,20))
        {
            destroyed = false;
            EnterTankMode();
        }
    }

    public void StartWave (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EventHandler.InvokeEvent(new NewWaveEvent(
                description: "Player started new wave"
            ));
        }
    }

    public void PauseGame (InputAction.CallbackContext context)
    {
        if (context.performed && !UI.MenuOpen)
        {
            UI ui = canvas.GetComponent<UI>();
            ui.SetSelectedButton("Resume");
            ui.PauseGame();
        }
    }

}
