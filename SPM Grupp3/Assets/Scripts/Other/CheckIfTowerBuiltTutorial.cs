using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfTowerBuiltTutorial : MonoBehaviour
{

    public GameManager theGameManager;

    public GameObject[] objectsToDisable;

    public GameObject[] objectsToEnable; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(theGameManager.towersPlaced.Count == 1)
        {
            foreach(GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }


            this.enabled = false;
        }
    }
}
