using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedUIElement : MonoBehaviour
{

    public int timeBeforeDeactivation;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        timeBeforeDeactivation -= 1; 

        if(timeBeforeDeactivation == 0)
        {
            gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
