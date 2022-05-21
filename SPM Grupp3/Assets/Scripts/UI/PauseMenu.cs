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
}