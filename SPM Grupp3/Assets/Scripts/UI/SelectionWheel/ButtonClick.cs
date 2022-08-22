using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonClick : MonoBehaviour
{
    public UnityEvent unityEvent;

    public bool buttonEnabled = true; 

    public void Click()
    {   
        if(buttonEnabled)
        {
            unityEvent.Invoke();
        }
       
    }
}
