using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject escapePanel;
    [SerializeField] private GameObject closeMessage;

    // Start is called before the first frame update
    void Start()
    {
        escapePanel.SetActive(false);
        closeMessage.SetActive(false);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Ska ändras till Esc, Tab för tillfället för att visa att musen kan bli osynlig
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!escapePanel.activeInHierarchy)
            {
                Cursor.visible = true;
                escapePanel.SetActive(true);
            }
            else if(!closeMessage.activeInHierarchy)
            {
                Cursor.visible = !Cursor.visible;
                escapePanel.SetActive(false);
            }
        }
    }

    public void ShowMessage()
    {
        closeMessage.SetActive(true);
    }

    public void CloseMessage()
    {
        closeMessage.SetActive(false);
    }

    public void CloseLevel() 
    {
        SceneManager.LoadScene(0);
    }
}
