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

  //  public List<GameObject> gameObjectsToStartDisabled = new List<GameObject>();

   // public GameManager gM; 

    public WaveManager scriptToActivate;
    public GameObject uiToActivate;
    private int timerForFirstEvent = 600;
    private bool firstEventActivated = false;
    private bool firstEventNotStarted = true;


    private int whichWave = 0;

    public GameObject FirstWaveActivation;
    public GameObject materialToActivate;

    public GameObject tileToActivate;


    public GameObject activateAfterSecondWave;



    public GameObject lastWaveActivate; 


    //private int whichWaveIsItOn; 


    // Start is called before the first frame update
    void Start()
    {   

        foreach (Transform child in parentOfTiles.transform)
        {
            child.gameObject.layer = 0; 
            listOfAllTiles.Add(child.gameObject) ; 
        }


   //     if(gameObjectsToStartDisabled.Count != 0 )
   //     {
   //         foreach (GameObject obj in gameObjectsToStartDisabled)
   //         {
   //
   //             obj.SetActive(false);
   //         }
   //
   //     }

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

                EventHandler.Instance.InvokeEvent(new NewWaveEvent(
        description: "New wave started",
        currentWave: 0
        ));
            }
        }

        /*
        if(scriptToActivate.isWaveGoingOn)
        {
            print("�r det en v�g p� g�ng " + scriptToActivate.isWaveGoingOn);
        }
        */
        
    }

    public void waveEnded()
    {   
        Debug.Log("waven slutade");
        switch (whichWave)
        {
            case 0:

                whichWave += 1; 
                break;
            case 1:

                FirstWaveActivation.SetActive(true);

                // Instantiate<>

                materialToActivate.SetActive(true);

                tileToActivate.layer = LayerMask.NameToLayer("PlaceForTower");

                whichWave += 1;
                break;
            case 2:

                activateAfterSecondWave.SetActive(true);

                whichWave += 1;
                break;
            case 3:

                
                break;
            case 4:
                lastWaveActivate.SetActive(true);
                whichWave += 1;

                break;
        }
    }

    
    
    public void activateFirstEvent()
    {
        firstEventActivated = true;





    }
}
