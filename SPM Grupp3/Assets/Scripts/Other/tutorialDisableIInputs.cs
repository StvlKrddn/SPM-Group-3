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



    [SerializeField] TutorialUI mainTutorial;
    [SerializeField] string[] inputsToEnableTankTutorialUI;

    [SerializeField] string[] inputsToEnableBuildTutorialUI;



    [SerializeField] string[] inputsToDisableTankTutorialUI;

    [SerializeField] string[] inputsToDisableBuildTutorialUI;

    // Start is called before the first frame update
    void Start()
    {
        mainTutorial = FindObjectOfType<TutorialUI>();

        playerInput = FindObjectOfType<PlayerInput>();
        foreach (string name in inputsToDisableUpdate)
        {
           // print(playerInput.actions[name]);

            playerInput.actions[name].Disable();

        }
        foreach (string name in inputsToEnableStart)
        {
            playerInput.actions[name].Enable();
        }
        // playerInput = FindObjectOfType<PlayerInput>();

        if (mainTutorial != null)
        {
            foreach(string obj in inputsToEnableTankTutorialUI)
            {

                mainTutorial.stopDisablingInputTankMode(obj);
            }
            foreach(string obj in inputsToEnableBuildTutorialUI)
            {
                mainTutorial.modesToDisableBuildMode.Remove(obj);

                playerInput.actions[obj].Enable();
            }

            foreach (string obj in inputsToDisableBuildTutorialUI)
            {
                mainTutorial.modesToDisableBuildMode.Add(obj);

                playerInput.actions[obj].Disable();
            }

            foreach (string obj in inputsToDisableTankTutorialUI)
            {
                mainTutorial.modesToDisableTankMode.Add(obj);


                playerInput.actions[obj].Disable();
            }

        }

        
    }

    // Update is called once per frame
    void Update()
    {
        //playerInput = FindObjectOfType<PlayerInput>();
        foreach (string name in inputsToDisableUpdate)
        {
            

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
