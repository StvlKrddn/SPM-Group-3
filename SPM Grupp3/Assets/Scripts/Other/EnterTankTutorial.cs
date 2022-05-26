using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterTankTutorial : MonoBehaviour
{

    public TutorialUI ScriptToCallFunction;

    public GameObject instructionsToDisplay; 
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; 
    }

    // Update is called once per frame
    void Update()
    {
       if(Gamepad.current.bButton.isPressed)
        {
            this.gameObject.SetActive(false);

            ScriptToCallFunction.activateFirstEvent();

            instructionsToDisplay.SetActive(true);




     //       FindObjectOfType<PlayerInput>().actio
        }
    }
}
