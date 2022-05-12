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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamepad.current.yButton.isPressed)
        {
            objectToDisable.SetActive(false);

            objectToEnable.SetActive(true);

            arrowToShow.SetActive(true);
        }
    }
}
