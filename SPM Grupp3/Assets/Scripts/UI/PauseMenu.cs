using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    void Awake() 
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = FindObjectOfType<MoveToMainCamera>().gameObject.GetComponent<Camera>();
        canvas.planeDistance = 10;
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!UI.IsPaused)
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                UI.IsPaused = true;
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        UI.IsPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        Resume();
        GameManager.Instance.RestartGame();
    }

    public void Continue()
    {
        Resume();
        GameManager.Instance.Continue();
    }

    public void Quit()
    {
        // NOTE(August): "Are you sure you want to quit?..." prompt?

        Resume();

        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();

        // Switch between the rows to build out
    }
}