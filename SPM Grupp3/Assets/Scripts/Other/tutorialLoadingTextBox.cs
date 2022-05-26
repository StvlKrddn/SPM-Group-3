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

    private InputAction enterGarageTest;

   // private PlayerInput playerInput; 

    void Start()
    {
        textToLoad = textBox.text;

        textBox.text = "";

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
        }

        audioSource.Stop();
        isTextLoading = false;

        yield return false;
    }




    // Update is called once per frame
    void Update()
    {
 
        if (isTextLoading)
        {

        }
        if(Gamepad.current.aButton.wasPressedThisFrame)
        {
            if(isTextLoading)
            {
                StopCoroutine(loadTextBox());

                isTextLoading = false;
                audioSource.Stop();
                textBox.text = textToLoad;
            }
            else
            {
                buttonToActivate.loadNextDialogue();
            }
        }
    }
}
