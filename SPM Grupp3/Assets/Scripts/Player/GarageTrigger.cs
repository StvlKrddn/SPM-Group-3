using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarageTrigger : MonoBehaviour
{
    private InputAction acceptAction;
    private GameObject hintEnterUI;
    private bool limit = false;

    void Awake() 
    {
        hintEnterUI = UI.Canvas.transform.Find("EnterGarageHint").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        hintEnterUI.SetActive(true);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tank"))
        {
            PlayerInput playerInput = other.GetComponentInParent<PlayerInput>();
            acceptAction = playerInput.actions["EnterGarage"];
            
            if (acceptAction.IsPressed() && limit == false)
            {
                hintEnterUI.SetActive(false);
                EventHandler.InvokeEvent(new PlayerSwitchEvent(
                    description: "A player switched mode",
                    playerContainer: other.transform.parent.gameObject
                    ));

                //hintEnterUI.SetActive(false);

            }
        }
        }

    private void OnTriggerExit(Collider other)
    {      
        hintEnterUI.SetActive(false);
    }

    public void ChangeLimit()
    {
        //limit = !limit; 
    }
}
