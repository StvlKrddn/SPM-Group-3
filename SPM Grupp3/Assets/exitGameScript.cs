using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class exitGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void exitGame()
    {
        EditorApplication.ExitPlaymode();
    }
}
