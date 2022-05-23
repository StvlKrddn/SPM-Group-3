using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelection;
    [SerializeField] private GameObject warningPrompt;

    private int sceneIndex;

    private void Awake() 
    {
        mainMenu.SetActive(true);
        levelSelection.SetActive(false);
        warningPrompt.SetActive(false);
        
        if (DataManager.FileExists(DataManager.SaveData))
        {
            mainMenu.transform.Find("Continue").gameObject.SetActive(true);
        }
    }
    
    public void SelectLevel(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
        if (DataManager.FileExists(DataManager.SaveData))
        {
            warningPrompt.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void Continue()
    {
        SaveData data = (SaveData) DataManager.ReadFromFile(DataManager.SaveData);
        SceneManager.LoadScene(data.currentScene);
    }

    public void Confirm()
    {
        DataManager.DeleteFile(DataManager.SaveData);
        SceneManager.LoadScene(sceneIndex);
    }

    public void Cancel()
    {
        warningPrompt.SetActive(false);
    }

    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}
