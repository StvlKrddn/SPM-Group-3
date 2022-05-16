using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarageTrigger : MonoBehaviour
{
    private InputAction acceptAction;

/*    [SerializeField] private GameObject hintEnterUI;*/
    
    [SerializeField] private GameObject hintEnterUI;

    private bool limit = false;




    private void OnTriggerEnter(Collider other)
    {
        //hintEnterUI.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tank"))
        {   
            PlayerInput playerInput = other.GetComponentInParent<PlayerInput>();
            acceptAction = playerInput.actions["EnterGarage"];
            print("vad �r v�rdet f�r limit " + limit);
            if (acceptAction.IsPressed() && !limit)
            {
                EventHandler.Instance.InvokeEvent(new PlayerSwitchEvent(
                    description: "A player switched mode",
                    playerContainer: other.transform.parent.gameObject
                    ));
                //hintEnterUI.SetActive(false);
            }
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        //hintEnterUI.SetActive(false);
    }

    public void ChangeLimit()
    {
        print("kommer den hit");
        limit = !limit; 
    }


}
