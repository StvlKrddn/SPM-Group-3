using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManagerScript : MonoBehaviour
{
    [SerializeField] private PlayerInputManager manager;

    public void OnPlayerJoined(PlayerInput obj)
    {
        if (obj.playerIndex == 0)
        {
            obj.name = "PlayerTank 1";
            obj.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            obj.name = "PlayerTank 2";
            obj.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
