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


    public GameObject disableTankInstructions;


    bool disableStartWaveTankMode = false;

    //private int whichWaveIsItOn; 
    private PlayerInput playerInput; 

    // Start is called before the first frame update
    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
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

        if(!disableStartWaveTankMode)
        {

            print(playerInput);


            if(playerInput.currentActionMap.name == "Tank" )
            {
                playerInput.actions["StartWave"].Disable();

                disableStartWaveTankMode = true;
            }
        }


        if (firstEventActivated && firstEventNotStarted)
        {   
            timerForFirstEvent -= 1;

            if(timerForFirstEvent == 0)
            {
                scriptToActivate.enabled = true;
                uiToActivate.SetActive(true);
                firstEventNotStarted = false;

            
                EventHandler.InvokeEvent(new NewWaveEvent(
                    description: "New wave started"
                ));
            }
        }

        /*
        if(scriptToActivate.isWaveGoingOn)
        {
            print("är det en våg på gång " + scriptToActivate.isWaveGoingOn);
        }
        */
        
    }

    public void waveEnded()
    {   
        Debug.Log("waven slutade");
        switch (whichWave)
        {
            case 0:
                FirstWaveActivation.SetActive(true);

                // Instantiate<>

                materialToActivate.SetActive(true);

                disableTankInstructions.SetActive(false); 

                tileToActivate.layer = LayerMask.NameToLayer("PlaceForTower");

        
                break;
            case 1:

                activateAfterSecondWave.SetActive(true);

                break;
           
            case 2:
                lastWaveActivate.SetActive(true);
              

                break;
          
        }

        whichWave += 1; 
    }

    
    
    public void activateFirstEvent()
    {
        firstEventActivated = true;





    }
}
