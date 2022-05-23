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

    private GarageTrigger garageTrigger; 

    // Start is called before the first frame update
    void Start()
    {
        garageTrigger = FindObjectOfType<GarageTrigger>();


        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        acceptAction = playerInput.actions["EnterGarage"];
        
    }

    private void OnTriggerStay(Collider other)
    {
       
        if (other.CompareTag("Tank"))
        {
            print("enablas den igen");
            if (trigger && !hasTriggered)
            {
                hasTriggered = true; 

                

                objectToDisable.SetActive(false);

                objectToEnable.SetActive(true);

                arrowToShow.SetActive(true);

                this.enabled = false;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        trigger = acceptAction.IsPressed(); 


    }
}
