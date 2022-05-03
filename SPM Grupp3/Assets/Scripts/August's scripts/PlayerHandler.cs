using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    private PlayerMode currentMode;

    [SerializeField] GameObject tankMode;
    [SerializeField] GameObject buildMode;
    [SerializeField] GameObject buildMenu;

    PlayerInput playerInput;

    void Awake()
    {
        EventHandler.Instance.RegisterListener<PlayerSwitchEvent>(OnPlayerSwitchMode);
        currentMode = FindObjectOfType<GameManager>().StartingMode;

        playerInput = GetComponent<PlayerInput>();

        if (currentMode == PlayerMode.Build)
        {
            // Disable Tank
            tankMode.SetActive(false);

            // Enable Build
            buildMode.SetActive(true);
        }

        if (currentMode == PlayerMode.Tank)
        {
            // Disable Build
            buildMode.SetActive(false);

            // Enable Tank
            tankMode.SetActive(true);
        }
    }

    private void OnPlayerSwitchMode(PlayerSwitchEvent eventInfo)
    {
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

            buildMenu.SetActive(true);

            currentMode = PlayerMode.Build;
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

            buildMenu.SetActive(false);
        }
    }


    // NOTE(August): Den här metoden bör ligga i ett UI script egentligen, men det finns inget passande just nu
    public void EnterTank()
    {
        EventHandler.Instance.InvokeEvent(new PlayerSwitchEvent(
            description: "Player entered tank",
            playerContainer: gameObject
            ));
    }
}
