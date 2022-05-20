using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static bool IsPaused;
    
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject pauseMenu;


    private static Canvas canvas;

    public static Canvas Canvas
    {
        get 
        {
            if (canvas == null)
            {
                canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();
            }
            return canvas;
        }
    }

    void Awake() 
    {
        canvas = GetComponent<Canvas>();
    }

    public void Restart()
    {
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        GameManager.Instance.RestartGame();
    }

    public void Continue()
    {
        victoryPanel.SetActive(false);
        GameManager.Instance.Continue();
    }

    public void Quit()
    {
        // NOTE(August): "Are you sure you want to quit?..." prompt?

        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();

        // Switch between the rows to build out
    }

    public static void OpenMenu()
    {        
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public static void CloseMenu()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }
}
