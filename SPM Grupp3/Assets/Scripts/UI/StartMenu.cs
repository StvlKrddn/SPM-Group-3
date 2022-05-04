using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject firstMenu;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject quitMessage;
    [SerializeField] private int sceneNumber;

    private void Start()
    {
        firstMenu.SetActive(true);
        optionMenu.SetActive(false);
        quitMessage.SetActive(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void OpenOptionMenu()
    {
        firstMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void CloseOptionMenu()
    {
        optionMenu.SetActive(false);
        firstMenu.SetActive(true);
    }

    public void ShowMessage()
    {
        quitMessage.SetActive(true);
    }

    public void CloseMessage()
    {
        quitMessage.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        quitMessage.SetActive(false);
    }
}
