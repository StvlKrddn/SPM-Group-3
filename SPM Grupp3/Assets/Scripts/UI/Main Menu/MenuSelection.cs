using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private GameObject nextSelectedButton;
    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(nextSelectedButton);
    }
}
