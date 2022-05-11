using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBuildCannonTower : MonoBehaviour
{   

    public GameObject[] gameObjectToEnableStart;

    public GameObject[] gameObjectToEnableEnd;

    public GameObject[] gameObjectToDisableEnd; 

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in gameObjectToEnableStart)
        {
            obj.SetActive(true);
        }

     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
