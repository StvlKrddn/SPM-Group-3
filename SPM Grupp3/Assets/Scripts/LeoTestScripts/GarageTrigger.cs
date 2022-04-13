using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarageTrigger : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction acceptAction;

    public delegate void EventHandler(TankController gameObject); // Used to create events that send data
    public event EventHandler OnTankEnterGarage;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TankController player = other.GetComponent<TankController>();
            playerInput = player.PlayerInput;
            acceptAction = playerInput.actions["Accept"];

            print("Enter Garage? (Press A)");
            if (acceptAction.IsPressed())
            {
                OnTankEnterGarage?.Invoke(player);
            }
        }   
    }
}
