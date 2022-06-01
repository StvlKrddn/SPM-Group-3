using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    [System.NonSerialized] public bool InBuildMode;
    [SerializeField] private Transform[] spawnPositions;
    private PlayerInputManager playerManager;
    private readonly List<PlayerHandler> players = new List<PlayerHandler>();

    void Awake()
    {
        EventHandler.RegisterListener<PlayerSwitchEvent>(SwitchPlayerMode);
        playerManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        print("New player joined the game");
        
        GameObject player = newPlayer.gameObject;

        players.Add(player.GetComponent<PlayerHandler>());

        EventHandler.InvokeEvent(new PlayerJoinedEvent(
            description: "A player joined the game",
            newPlayerGO: player
        ));
    }

    public void SwitchPlayerMode(PlayerSwitchEvent eventInfo)
    {
        PlayerHandler playerHandler = eventInfo.PlayerContainer.GetComponent<PlayerHandler>();
        playerHandler.SwitchMode();
        if (playerManager.playerCount < 2)
        {
            InBuildMode = players[0].CurrentMode == PlayerMode.Build;
        }
        else
        {
            InBuildMode = players[0].CurrentMode == PlayerMode.Build || players[1].CurrentMode == PlayerMode.Build;
        }
    }
}
