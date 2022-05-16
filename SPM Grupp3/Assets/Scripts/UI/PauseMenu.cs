using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public static bool IsPaused;

    private List<GameObject> buttons = new List<GameObject>();

    void Awake() 
    {
        foreach (Transform button in pauseMenu.transform)
        {
            buttons.Add(button.gameObject);
        }
    }

    private void Update() 
    {
        print(EventSystem.current.currentSelectedGameObject.name);
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!IsPaused)
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                IsPaused = true;
            }
            else
            {
                Resume();
            }
        }
        
    }

    public void Resume()
    {
        IsPaused = false;
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
        GameManager.Instance.ContinueGame();
    }

    public void Quit()
    {
        // NOTE(August): "Are you sure you want to quit?..." prompt?

        Resume();

        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();

        // Switch between the rows to build out
    }

    public static void OpenMenu()
    {        
        IsPaused = true;
        Time.timeScale = 0f;
    }
}
