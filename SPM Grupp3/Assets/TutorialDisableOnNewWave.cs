using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.Events;
public class TutorialDisableOnNewWave : MonoBehaviour
{

    public GameObject[] gameObjectsToDisable; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamepad.current.xButton.isPressed)
        {
            gameObject.SetActive(false);

            foreach(GameObject obj in gameObjectsToDisable)
            {
                obj.SetActive(false);
            }
        }
    }
}
