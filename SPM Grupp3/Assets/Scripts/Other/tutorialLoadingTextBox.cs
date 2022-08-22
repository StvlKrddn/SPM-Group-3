using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;


public class tutorialLoadingTextBox : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Text textBox;

    private string textToLoad;

    [SerializeField] float speedOfTextLoad;

    private bool isTextLoading = false;

    public  TutorialButton buttonToActivate;

    public AudioSource audioSource;

    private PlayerInput playerInput;
    private InputAction progressDialogue;

    private bool isPaused; 


    private TutorialUI tutorialUI;
   // private PlayerInput playerInput; 

    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        textToLoad = textBox.text;
        tutorialUI = FindObjectOfType<TutorialUI>();
        textBox.text = "";

        progressDialogue = playerInput.actions["ProgressDialouge"];

        StartCoroutine(loadTextBox());
        
        
 

    //    playerInput.actions["StartWave"];

    
     
     //   enterGarageTest.Disable();
    }



    private IEnumerator loadTextBox()
    {
        isTextLoading = true;
        
        audioSource.Play();

        for (int i = 0; i < textToLoad.Length; i++)
        {
            textBox.text += textToLoad[i];

            yield return new WaitForSeconds(speedOfTextLoad);

            if(progressDialogue.triggered)
            {
                isTextLoading = false;
                audioSource.Stop();
                textBox.text = textToLoad;

                yield break;
            }
        }

        audioSource.Stop();
        isTextLoading = false;

        yield return false;
    }




    public void onPause()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(tutorialUI.isPaused)
        {
            isPaused = tutorialUI.isPaused;

            
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1;
            

            isPaused = false;
        }

        

        if (isTextLoading)
        {

        }
        if(//progressDialogue.triggered ||
            Gamepad.current.aButton.wasPressedThisFrame)
        {
            if(isTextLoading)
            {
                if(!isPaused)
                {
                    StopCoroutine(loadTextBox());
                    StopAllCoroutines();
                    isTextLoading = false;
                    audioSource.Stop();
                    textBox.text = textToLoad;
                }

            }
            else
            {
                if(buttonToActivate != null && !isPaused)
                {
                    buttonToActivate.loadNextDialogue();
                }
             
            }
        }
    }
}
