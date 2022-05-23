using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.Events;

public class TutorialEnterGarage : MonoBehaviour
{

    public GameObject objectToDisable;

    public GameObject objectToEnable;

    public GameObject arrowToShow;

    private InputAction acceptAction;

    private bool trigger;

    private bool hasTriggered = false;

    private bool hasItBeenRightInput; 

    private GarageTrigger garageTrigger;

    public GameObject objectGarageTriggerEnable; 

    // Start is called before the first frame update
    void Start()
    {
        garageTrigger = FindObjectOfType<GarageTrigger>();


        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        acceptAction = playerInput.actions["EnterGarage"];
        
    }

    private void OnTriggerStay(Collider other)
    {
     //   trigger = acceptAction.IsPressed();
        if (other.CompareTag("Tank"))
        {
            print("vad är trigger " + trigger + " vad är hasTriggered " + hasTriggered);
            if (hasItBeenRightInput && !hasTriggered)
            {
                hasTriggered = true; 

                

                objectToDisable.SetActive(false);

                objectToEnable.SetActive(true);

                arrowToShow.SetActive(true);


                objectGarageTriggerEnable.SetActive(true);

                

                this.enabled = false;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        trigger = acceptAction.IsPressed();

        

        hasItBeenRightInput = trigger;

        print(trigger);

    }
}
