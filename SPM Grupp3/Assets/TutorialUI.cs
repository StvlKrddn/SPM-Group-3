using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.Events;



public class TutorialUI : MonoBehaviour
{
  //  public Text ShowAmountOfTiles;

    public GameObject parentOfTiles;

    private List<GameObject> listOfAllTiles = new List<GameObject>();


    public WaveManager scriptToActivate;
    public GameObject uiToActivate;
    private int timerForFirstEvent = 600;
    private bool firstEventActivated = false;
    private bool firstEventNotStarted = true;

    

    // Start is called before the first frame update
    void Start()
    {   

        foreach (Transform child in parentOfTiles.transform)
        {
            child.gameObject.layer = 0; 
            listOfAllTiles.Add(child.gameObject) ; 
        }

        

   //     ShowAmountOfTiles.text = "hej " + listOfAllTiles.Count;


        //Time.timeScale = 0; 
    }



   

    // Update is called once per frame
    void Update()
    {
        if (firstEventActivated && firstEventNotStarted)
        {   
            timerForFirstEvent -= 1;

            if(timerForFirstEvent == 0)
            {
                scriptToActivate.enabled = true;
                uiToActivate.SetActive(true);
                firstEventNotStarted = false;
            }
        }


        if(scriptToActivate.isWaveGoingOn)
        {
            print("är det en våg på gång " + scriptToActivate.isWaveGoingOn);
        }
        
    }


    public void activateFirstEvent()
    {
        firstEventActivated = true; 
    }
}
