using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarageTrigger : MonoBehaviour
{
    private InputAction acceptAction;
    private GameObject hintEnterUI;
    private readonly bool limit = false;

    [SerializeField] private FadeBehaviour fadeBehaviour;

    void Awake() 
    {
        hintEnterUI = UI.Canvas.transform.Find("EnterGarageHint").gameObject;
    }

    public void ShowHover()
    {
        fadeBehaviour.Fade();
    }

    public void HideHover()
    {   
        fadeBehaviour.Fade();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tank"))
        {
            PlayerInput playerInput = other.GetComponentInParent<PlayerInput>();
            acceptAction = playerInput.actions["EnterGarage"];

            if(hintEnterUI.GetComponent<FadeBehaviour>().Faded())
                hintEnterUI.GetComponent<FadeBehaviour>().Fade();

            if (acceptAction.IsPressed() && limit == false)
            {
                if (!hintEnterUI.GetComponent<FadeBehaviour>().Faded())
                    hintEnterUI.GetComponent<FadeBehaviour>().Fade();

                EventHandler.InvokeEvent(new PlayerSwitchEvent(
                    description: "A player switched mode",
                    playerContainer: other.transform.parent.gameObject
                    ));

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tank"))
        {
            if(!hintEnterUI.GetComponent<FadeBehaviour>().Faded())
                hintEnterUI.GetComponent<FadeBehaviour>().Fade();
        }
    }
}
