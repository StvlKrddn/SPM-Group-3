using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerManager playerManager;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        
    }

    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        GameObject player = newPlayer.gameObject;
    }
}
