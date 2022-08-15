using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Sprite normalCursor;
    [SerializeField] private Sprite clickedCursor;
    private Animator cursorAnimator;
    private Image cursorImage;
    private bool isClicking;

    private void Awake()
    {
        cursorAnimator = GetComponent<Animator>();
        cursorImage = GetComponent<Image>();
        cursorImage.sprite = normalCursor;
    }

    public void ToggleClick(bool isClicked)
    {
        cursorImage.sprite = isClicked ? cursorImage.sprite = clickedCursor : cursorImage.sprite = normalCursor;
    }

    public void ShowCursor()
    {
        cursorAnimator.Play("Appear");
    }

    public void HideCursor()
    {
        cursorAnimator.SetTrigger("Disappear");
    }

    private void DeActivate()
    {
        gameObject.SetActive(false);
    }

    private void Activate()
    {
        gameObject.SetActive(true);
    }
}
