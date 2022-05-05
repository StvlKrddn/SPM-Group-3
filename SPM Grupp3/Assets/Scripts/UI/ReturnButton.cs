using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class ReturnButton : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject canvasToDeactivate;
    public GameObject canvasToActivate;

    bool bButtonPressed;

    void Start()
    {
        
    }

    public void OnClick()
    {
        canvasToDeactivate.SetActive(false);
        canvasToActivate.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        bButtonPressed = Gamepad.current.bButton.IsPressed();

        if(bButtonPressed)
        {
            canvasToDeactivate.SetActive(false);
            canvasToActivate.SetActive(true);
        }
    }
}
