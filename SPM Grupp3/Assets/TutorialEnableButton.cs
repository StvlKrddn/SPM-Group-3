using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnableButton : MonoBehaviour
{

    public ButtonClick buttonToEnable; 
    // Start is called before the first frame update
    void Start()
    {
        buttonToEnable.buttonEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
