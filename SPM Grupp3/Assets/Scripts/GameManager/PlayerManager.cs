using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager playerManager;
    private GameObject playerPrefab;

    void Awake()
    {
        EventHandler.Instance.RegisterListener<PlayerSwitchEvent>(SwitchPlayerMode);
        playerManager = GetComponent<PlayerInputManager>();
        playerPrefab = playerManager.playerPrefab;
    }

    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        GameObject player = newPlayer.gameObject;

        EventHandler.Instance.InvokeEvent(new PlayerJoinedEvent(
            description: "A player joined the game",
            newPlayerGO: player
        ));
    }

    public void SwitchPlayerMode(PlayerSwitchEvent eventInfo)
    {
        PlayerHandler playerHandler = eventInfo.PlayerContainer.GetComponent<PlayerHandler>();
        playerHandler.SwitchMode();
    }
}
