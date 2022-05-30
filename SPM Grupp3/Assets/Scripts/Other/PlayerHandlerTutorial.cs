using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerHandlerTutorial : MonoBehaviour
{
    private PlayerMode currentMode;
    private GameObject canvas;
    private PlayerInput playerInput;
    public GameObject tankMode;
    private GameObject buildMode;

    void Start()
    {
        canvas = Camera.main.transform.Find("Canvas").gameObject;

        currentMode = FindObjectOfType<GameManager>().StartingMode;

        playerInput = GetComponent<PlayerInput>();

        playerInput.uiInputModule = GetComponent<InputSystemUIInputModule>();

        tankMode = transform.Find("TankMode").gameObject;
        buildMode = transform.Find("BuilderMode").gameObject;

        // First mode set up
        if (currentMode == PlayerMode.Build)
        {
            EnterBuildMode();
        }
        else
        {
            EnterTankMode();
        }
    }

    void EnterTankMode()
    {
        // Disable Build
        buildMode.SetActive(false);

        // Enable Tank
        tankMode.SetActive(true);

        EventHandler.InvokeEvent(new EnterTankModeEvent(
            description: "Player entered tank mode",
            playerContainer: gameObject
            ));

        playerInput.SwitchCurrentActionMap("Tank");

        currentMode = PlayerMode.Tank;

    }

    void EnterBuildMode()
    {
        // Disable Tank
        tankMode.SetActive(false);

        // Enable Build
        buildMode.SetActive(true);

        EventHandler.InvokeEvent(new EnterBuildModeEvent(
            description: "Player entered build mode",
            player: gameObject
            ));

        playerInput.SwitchCurrentActionMap("Builder");

        currentMode = PlayerMode.Build;

    }

    public void SwitchMode()
    {
        // If in Tank mode, switch to Build
        if (currentMode == PlayerMode.Tank)
        {
            EnterBuildMode();
        }
        // If in Build mode, switch to Tank
        else
        {
            EnterTankMode();
        }
    }

    public void StartWave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EventHandler.InvokeEvent(new StartWaveEvent(
                description: "Player started new wave",
                invoker: gameObject
            ));
        }
    }
}
