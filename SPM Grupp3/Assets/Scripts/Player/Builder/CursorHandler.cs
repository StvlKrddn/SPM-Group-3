using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Sprite normalCursor;
    [SerializeField] private Sprite clickedCursor;
    private Image cursorImage;
    private bool isClicking;

    private void Awake()
    {
        cursorImage = GetComponent<Image>();
        cursorImage.sprite = normalCursor;
    }

    private void OnEnable()
    {
        print("varfor sker detta");

        int test = 5; 
    }

    public void ToggleClick(bool isClicked)
    {
        cursorImage.sprite = isClicked ? cursorImage.sprite = clickedCursor : cursorImage.sprite = normalCursor;
    }
}
