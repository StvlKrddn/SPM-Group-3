using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private static bool isPaused;

    public static bool IsPaused { get { return isPaused; } }

    void Awake() 
    {
        EventHandler.Instance.RegisterListener<VictoryEvent>(OnVictory);
        EventHandler.Instance.RegisterListener<DefeatEvent>(OnDefeat);    
    }

    public static void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public static void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        ResumeGame();

        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();

        // Switch between the rows to build out
    }

    private void OnVictory(VictoryEvent eventInfo)
    {
        PauseGame();
    }

    private void OnDefeat(DefeatEvent eventInfo)
    {
        PauseGame();
    }
}
