using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    
    private static Canvas canvas;

    public static Canvas Canvas
    {
        get 
        {
            if (canvas == null)
            {
                canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();
            }
            return canvas;
        }
    }

    void Awake() 
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        
    }
}
