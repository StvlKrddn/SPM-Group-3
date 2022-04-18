using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarageTrigger : MonoBehaviour
{
    private InputAction acceptAction;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TankController player = other.GetComponent<TankController>();
            acceptAction = player.PlayerInput.actions["Accept"];

            print("Enter Garage? (Press A)");
            if (acceptAction.IsPressed())
            {
                print("Entered Garage");
                player.PlayerInput.SwitchCurrentActionMap("Parked");
                EventHandler.Instance.InvokeEvent(new GarageEvent(
                    description: "A player entered the garage",
                    player: other.gameObject
                    ));
            }
        }   
    }
}
