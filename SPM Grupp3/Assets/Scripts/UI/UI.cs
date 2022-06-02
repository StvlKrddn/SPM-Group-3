using System.Reflection;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static bool IsPaused;
    public static bool MenuOpen;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Text waveCounter;

    private GameObject resumeButton;
    private GameObject restartButton;
    private GameObject continueButton;
    private EventSystem eventSystem;

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
        resumeButton = pauseMenu.transform.Find("Resume").gameObject;
        continueButton = victoryPanel.transform.Find("Buttons").Find("ContinueButton").gameObject;
        restartButton = defeatPanel.transform.Find("Buttons").Find("RestartButton").gameObject;

        eventSystem = FindObjectOfType<EventSystem>();
    }

    public void PauseGame()
    {
        if (!IsPaused)
        {
            MusicManager.instance.SetMusicPLay(false);
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            IsPaused = true;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
        else
        {
            Resume();
            
        }

    }

    public void Restart()
    {
        Resume();
        CloseMenu();
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        GameManager.Instance.DeleteSaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Continue()
    {
        Resume();
        CloseMenu();
        victoryPanel.SetActive(false);

        DataManager.DeleteFile(DataManager.SaveData);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if ((sceneIndex + 1) > 3)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
    }

    public void Quit()
    {
        // NOTE(August): "Are you sure you want to quit?..." prompt?
        Resume();
        CloseMenu();
        UpgradeController.Instance.ResetUpgrades();
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        MusicManager.instance.SetMusicPLay(true);
        IsPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public static void OpenMenu()
    {
        MenuOpen = true;
        Time.timeScale = 0f;
    }

    public static void CloseMenu()
    {
        MenuOpen = false;
        Time.timeScale = 1f;
    }

    public void SetFirstSelectedButton(string buttonName)
    {
        switch (buttonName)
        {
            case "Resume":
                SetSelectedButton(resumeButton);
                break;
            case "Continue":
                SetSelectedButton(continueButton);
                break;
            case "Restart":
                SetSelectedButton(restartButton);
                break;
        }
    }

    public void SetSelectedButton(GameObject button)
    {
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(button);
    }

}
