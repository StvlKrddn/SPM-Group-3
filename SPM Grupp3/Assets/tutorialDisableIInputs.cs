using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
public class tutorialDisableIInputs : MonoBehaviour
{

    [SerializeField] string[] inputsToDisableUpdate;
    [SerializeField] string[] inputsToEnableStart;



    private PlayerInput playerInput; 

    [SerializeField] string[] inputsToEnableFunction;
    [SerializeField] string[] inputsToDisableFunction;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        foreach (string name in inputsToDisableUpdate)
        {
            print(playerInput.actions[name]);

         //   playerInput.actions[name].Disable();

        }
        foreach (string name in inputsToEnableStart)
        {
            playerInput.actions[name].Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //playerInput = FindObjectOfType<PlayerInput>();
        foreach (string name in inputsToDisableUpdate)
        {
            print(playerInput.actions[name]);

             playerInput.actions[name].Disable();

        }
        foreach (string name in inputsToEnableStart)
        {
            playerInput.actions[name].Enable();
        }
    }

    public void enableInputs()
    {

    }

    public void disableInputs()
    {

    }

    
}
