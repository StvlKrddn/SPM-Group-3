using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject warningPrompt;

    [SerializeField] private Animator menuAnimator;
    [SerializeField] private Animator fadeAnimator;

    private GameObject mainMenu;
    private int sceneIndex;
    private bool hatNoticeShown;

    private bool hasIntroduced = false;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pressAnyKeyClip;

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

    private void Update()
    {
        if (Input.anyKey && !hasIntroduced)
        {
            menuAnimator.SetTrigger("Introduction");

            audioSource.PlayOneShot(pressAnyKeyClip);

            hasIntroduced = true;
        }
    }

    public void SelectLevel(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
        if (DataManager.FileExists(DataManager.SaveData))
        {
            //warningPrompt.SetActive(true);
            menuAnimator.SetTrigger("ToWarning");
        }
        else
        {
            //SceneManager.LoadScene(sceneIndex);
            StartCoroutine(LoadLevel(sceneIndex));
        }
    }

    public void Continue()
    {
        SaveData data = (SaveData) DataManager.ReadFromFile(DataManager.SaveData);
        //SceneManager.LoadScene(data.CurrentScene);
        StartCoroutine(LoadLevel(data.CurrentScene));
    }

    public void Confirm()
    {
        DataManager.DeleteFile(DataManager.SaveData);
        //SceneManager.LoadScene(sceneIndex);
        menuAnimator.SetTrigger("ToSelection");
        StartCoroutine(LoadLevel(sceneIndex));
    }

    public void Cancel()
    {
        //warningPrompt.SetActive(false);
        menuAnimator.SetTrigger("ToSelection");
    }

    public void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
        StartCoroutine(LoadLevel(-1));
    }

    public void Reset()
    {
        GameObject continueButton = mainMenu.transform.Find("Continue").gameObject;
        if (continueButton.active == true)
            StartCoroutine(DisableButton(continueButton));

        DataManager.DeleteFile(DataManager.SaveData);
        DataManager.DeleteFile(DataManager.CustomizationData);
        DataManager.DeleteFile(DataManager.AchievementData);
    }

    private IEnumerator DisableButton(GameObject button)
    {
        button.GetComponent<Button>().interactable = false;
        float time = 0;

        while(time <= 2)
        {
            time += Time.deltaTime;
            button.transform.localScale = Vector3.one * Mathfx.Berp(1f, 0f, time);

            yield return null;
        }

        button.SetActive(false);
        button.transform.localScale = Vector3.one;
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        fadeAnimator.SetTrigger("StartFade");

        yield return new WaitForSeconds(4);

        if (levelIndex >= 0)
            SceneManager.LoadScene(levelIndex);
        else
            Application.Quit();
    }
}
