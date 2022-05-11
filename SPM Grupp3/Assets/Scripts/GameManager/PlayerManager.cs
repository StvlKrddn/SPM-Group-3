using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject buildMenu;
    private PlayerInputManager playerManager;
    
    private List<PlayerHandler> players = new List<PlayerHandler>();

    [System.NonSerialized] public bool InBuildMode;

    void Awake()
    {
        EventHandler.Instance.RegisterListener<PlayerSwitchEvent>(SwitchPlayerMode);
        playerManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        print("New player joined the game");
        
        GameObject player = newPlayer.gameObject;
        players.Add(player.GetComponent<PlayerHandler>());

        EventHandler.Instance.InvokeEvent(new PlayerJoinedEvent(
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
        buildMenu.SetActive(InBuildMode);
    }
}
