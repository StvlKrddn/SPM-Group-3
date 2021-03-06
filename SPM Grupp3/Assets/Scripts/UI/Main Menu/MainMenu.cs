using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject warningPrompt;

    private GameObject mainMenu;
    private int sceneIndex;
    private bool hatNoticeShown;

    private void Awake() 
    {
        //Set only MainMenu as active
        foreach (GameObject item in menuItems)
        {
            if (item.name.Equals("MainMenu"))
            {
                mainMenu = item;
                mainMenu.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
        
        if (DataManager.FileExists(DataManager.SaveData))
        {
            GameObject continueButton = mainMenu.transform.Find("Continue").gameObject;
            continueButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(continueButton);
        }
        if (!hatNoticeShown && AchievementTracker.Instance.IsAchievementCompleted(Achievement.CompleteStageThree))
        {
            hatNoticeShown = true;
            mainMenu.transform.Find("NewHatNotice").gameObject.SetActive(true);
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
        SceneManager.LoadScene(data.CurrentScene);
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
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void Reset()
    {
        DataManager.DeleteFile(DataManager.SaveData);
        DataManager.DeleteFile(DataManager.CustomizationData);
        DataManager.DeleteFile(DataManager.AchievementData);
    }
}
