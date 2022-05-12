using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    public int timerBeforeDisable = 0;

    // Start is called before the first frame update
    void Start()
    {
        



    }


    private void FixedUpdate()
    {
        timerBeforeDisable -= 1;

        if(timerBeforeDisable == 0)
        {

            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
     


    }
}
