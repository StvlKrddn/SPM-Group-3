using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageTrigger : MonoBehaviour
{

    [SerializeField] private GameObject buildingUI;

    // Start is called before the first frame update
    void Start()
    {
        // Sätter Panelen till osynlig
        buildingUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buildingUI.SetActive(true);
            //other.gameObject.SetActive(false);
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buildingUI.SetActive(false);
            //other.gameObject.SetActive(false);
        }
    }
}
