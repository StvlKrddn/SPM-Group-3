using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarageTrigger : MonoBehaviour
{
    private InputAction acceptAction;
    
    private void OnTriggerEnter(Collider other)
    {
        print("Enter Garage? (Press X)");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tank"))
        {
            PlayerInput playerInput = other.GetComponentInParent<PlayerInput>();
            acceptAction = playerInput.actions["EnterGarage"];

            if (acceptAction.IsPressed())
            {
                EventHandler.Instance.InvokeEvent(new PlayerSwitchEvent(
                    description: "A player switched mode",
                    playerContainer: other.transform.parent.gameObject
                    ));
            }
        }   
    }

}
