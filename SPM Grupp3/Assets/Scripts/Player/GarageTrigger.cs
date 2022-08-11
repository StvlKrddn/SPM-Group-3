using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarageTrigger : MonoBehaviour
{
    private InputAction acceptAction;
    
    private readonly bool limit = false;

    private FadeBehaviour fadeUI;
    private AppearBehaviour appearUI;
    [SerializeField] private FadeBehaviour OptionGarageIndicator;

    void Awake() 
    {
        GameObject hintEnterUI = UI.Canvas.transform.Find("EnterGarageHint").gameObject;
        fadeUI = hintEnterUI.GetComponent<FadeBehaviour>();
        appearUI = hintEnterUI.GetComponent<AppearBehaviour>();
    }

    public void ShowIndicator()
    {
        if(OptionGarageIndicator.Faded())
            OptionGarageIndicator.Fade();
    }

    public void CloseIndicator()
    {
        if (!OptionGarageIndicator.Faded())
            OptionGarageIndicator.Fade();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tank"))
        {
            PlayerInput playerInput = other.GetComponentInParent<PlayerInput>();
            acceptAction = playerInput.actions["EnterGarage"];

            if (fadeUI.Faded())
            {
                fadeUI.Fade();
                appearUI.Appear();
            }
                

            if (acceptAction.IsPressed() && limit == false)
            {
                if (!fadeUI.Faded())
                {
                    fadeUI.Fade();
                    appearUI.Disappear();
                }
                    

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
            if (!fadeUI.Faded())
            {
                fadeUI.Fade();
                appearUI.Disappear();
            }
        }
    }
}
