using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{

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
        // Ska �ndras till Esc, Tab f�r tillf�llet f�r att visa att musen kan bli osynlig
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!escapePanel.active)
            {
                Cursor.visible = true;
                escapePanel.SetActive(true);
            }
            else if(!closeMessage.active)
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
