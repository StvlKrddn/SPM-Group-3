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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapePanel.SetActive(!escapePanel.activeInHierarchy);
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
