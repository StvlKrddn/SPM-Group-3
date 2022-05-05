using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    private PlayerMode currentMode;

    GameObject canvas;
    PlayerInput playerInput;
    GameObject tankMode;
    GameObject buildMode;

    void Start()
    {
        canvas = Camera.main.transform.Find("Canvas").gameObject;

        EventHandler.Instance.RegisterListener<PlayerSwitchEvent>(OnPlayerSwitchMode);
        currentMode = FindObjectOfType<GameManager>().StartingMode;

        playerInput = GetComponent<PlayerInput>();

        tankMode = transform.Find("TankMode").gameObject;
        buildMode = transform.Find("BuilderMode").gameObject;

        // First mode set up
        if (currentMode == PlayerMode.Build)
        {
            // Disable Tank
            tankMode.SetActive(false);

            // Enable Build
            buildMode.SetActive(true);

            playerInput.SwitchCurrentActionMap("Builder");

            canvas.transform.GetChild(1).gameObject.SetActive(true);

            currentMode = PlayerMode.Build;
        }

        if (currentMode == PlayerMode.Tank)
        {
            // Disable Build
            buildMode.SetActive(false);

            // Enable Tank
            tankMode.SetActive(true);

            playerInput.SwitchCurrentActionMap("Tank");

            currentMode = PlayerMode.Tank;

            canvas.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void OnPlayerSwitchMode(PlayerSwitchEvent eventInfo)
    {
        tankMode = eventInfo.PlayerContainer.transform.Find("TankMode").gameObject;
        buildMode = eventInfo.PlayerContainer.transform.Find("BuilderMode").gameObject;
        // If in Tank mode, switch to Build
        if (currentMode == PlayerMode.Tank)
        {
            // Disable Tank
            tankMode.SetActive(false);

            // Enable Build
            buildMode.SetActive(true);

            EventHandler.Instance.InvokeEvent(new EnterBuildModeEvent(
                description: "Player entered new mode",
                player: eventInfo.PlayerContainer
                ));

            playerInput.SwitchCurrentActionMap("Builder");

            canvas.transform.GetChild(1).gameObject.SetActive(true);

            currentMode = PlayerMode.Build;

            print("Entered build mode");
        }

        // If in Build mode, switch to Tank
        else
        {
            // Disable Build
            buildMode.SetActive(false);

            // Enable Tank
            tankMode.SetActive(true);

            EventHandler.Instance.InvokeEvent(new EnterTankModeEvent(
                description: "Player entered new mode",
                playerContainer: eventInfo.PlayerContainer
                ));

            playerInput.SwitchCurrentActionMap("Tank");

            currentMode = PlayerMode.Tank;

            canvas.transform.GetChild(1).gameObject.SetActive(false);

            print("Entered tank mode");
        }
    }

    public void EnterTank()
    {
        EventHandler.Instance.InvokeEvent(new PlayerSwitchEvent(
            description: "Player entered tank",
            playerContainer: gameObject
            ));
    }
}
