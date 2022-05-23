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

/*    [SerializeField] private GameObject tankMode;
    [SerializeField] private GameObject buildMode;*/

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

        // Enable Tank
        tankMode.SetActive(true);

        EventHandler.Instance.InvokeEvent(new EnterTankModeEvent(
            description: "Player entered tank mode",
            playerContainer: gameObject
            ));

        playerInput.SwitchCurrentActionMap("Tank");

        GetComponentInChildren<Health>().ResetHealth();

        //canvas.transform.Find("Build_UI").gameObject.SetActive(false);

        currentMode = PlayerMode.Tank;

        UpgradeController.Instance.FixUpgrades(gameObject);
    }

    void EnterBuildMode()
    {
        // Disable Tank
        tankMode.SetActive(false);

        // Enable Build
        buildMode.SetActive(true);

        EventHandler.Instance.InvokeEvent(new EnterBuildModeEvent(
            description: "Player entered build mode",
            player: gameObject
            ));

        playerInput.SwitchCurrentActionMap("Builder");

        //canvas.transform.Find("Build_UI").gameObject.SetActive(true);

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
            print("Your tank is destroyed! Wait until next wave");
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
            EventHandler.Instance.InvokeEvent(new StartWaveEvent(
                description: "Player started new wave",
                invoker: gameObject
            ));
        }
    }

    public void PauseGame (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UI ui = canvas.GetComponent<UI>();
            ui.SetSelectedButton("Resume");
            ui.PauseGame();
        }
    }

}
