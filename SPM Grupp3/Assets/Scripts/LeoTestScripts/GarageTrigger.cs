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
            TankState player = other.GetComponent<TankState>();
            acceptAction = player.PlayerInput.actions["Accept"];

            print("Enter Garage? (Press A)");
            if (acceptAction.IsPressed())
            {
                print("Entered Garage");
                EventHandler.Instance.InvokeEvent(new GarageEvent(
                    description: "A player entered the garage",
                    player: other.gameObject
                    ));
            }
        }   
    }
}
