using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.Events;

public class ButtonMethodSelection : MonoBehaviour
{

    public UnityEvent whichShopMethod;
    private bool isAButtonPressed;
    private bool isYButtonPressed;

    public GameObject canvasToDisplay;
    public GameObject canvasToDeactivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        if (whichShopMethod == null)
            whichShopMethod = new UnityEvent();
    }

    

    public void OnClick()
    {
        if(isAButtonPressed)
        {
            whichShopMethod.Invoke();
        }

        if(isYButtonPressed)
        {
            canvasToDeactivate.SetActive(false);
            canvasToDisplay.SetActive(true);
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        isAButtonPressed = Gamepad.current.aButton.IsPressed();
        isYButtonPressed = Gamepad.current.yButton.IsPressed();
    }
}
