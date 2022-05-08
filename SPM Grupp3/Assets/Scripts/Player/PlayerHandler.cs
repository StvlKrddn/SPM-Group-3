using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    private PlayerMode currentMode;
    private GameObject canvas;
    private PlayerInput playerInput;
    private GameObject tankMode;
    private GameObject buildMode;

    void Start()
    {
        canvas = Camera.main.transform.Find("CanvasV2").gameObject;

        currentMode = FindObjectOfType<GameManager>().StartingMode;

        playerInput = GetComponent<PlayerInput>();

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

        EventHandler.Instance.InvokeEvent(new EnterTankModeEvent(
            description: "Player entered tank mode",
            playerContainer: gameObject
            ));

        playerInput.SwitchCurrentActionMap("Tank");

        currentMode = PlayerMode.Tank;

        print("Entered tank mode");
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

        currentMode = PlayerMode.Build;

        print("Entered build mode");
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
}
