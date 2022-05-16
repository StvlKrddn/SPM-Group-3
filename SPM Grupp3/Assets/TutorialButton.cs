using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{


    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void startGame()
    {
        Time.timeScale = 1; 
    }

    public void loadNextDialogue()
    {
        if(objectsToDisable.Length != 0)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
        }

        if(objectsToEnable.Length != 0)
        {
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }
        }      
    }
}
