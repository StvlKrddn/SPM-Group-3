using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageTrigger : MonoBehaviour
{

    [SerializeField] private GameObject buildingUI;
    [SerializeField] private Texture2D cursorImage;

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
            Cursor.visible = true;
            // Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
            //other.gameObject.SetActive(false);
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buildingUI.SetActive(false);
            Cursor.visible = false;
            //other.gameObject.SetActive(false);
        }
    }
}
