using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

/*
 * This class is based on this YouTube tutorial: https://youtu.be/Y3WNwl1ObC8
 */

public class BuilderController : MonoBehaviour
{
    [SerializeField] private float cursorSpeed = 1000f;
    [Space]
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private Transform canvas;

    Mouse virtualMouse;
    PlayerInput playerInput;
    InputAction stickAction;
    InputAction acceptAction;

    bool previousMouseState;

    void Awake()
    {
        InputSystem.onAfterUpdate += UpdateVirtualMouse;
        EventHandler.Instance.RegisterListener<EnterBuildModeEvent>(EnterBuildMode);

        playerInput = transform.parent.GetComponent<PlayerInput>();
        stickAction = playerInput.actions["Cursor"];
        acceptAction = playerInput.actions["Accept"];

        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }

        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }
    }

    void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateVirtualMouse;
        //EventHandler.Instance.UnregisterListener<GarageEvent>(EnterBuildMode);
    }

    void UpdateVirtualMouse()
    {
        if (virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 newPosition = MoveMouse();
        UpdateCursorImage(newPosition);
        CheckIfClicked();
    }


    Vector2 MoveMouse()
    {
        Vector2 cursorMovement = Gamepad.current.leftStick.ReadValue();
        cursorMovement *= cursorSpeed * Time.unscaledDeltaTime;
        Vector2 newPosition = virtualMouse.position.ReadValue() + cursorMovement;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, cursorMovement);

        return newPosition;
    }

    void CheckIfClicked()
    {
        bool isAcceptPressed = Gamepad.current.aButton.IsPressed();

        // If the button is not already pressed
        if (previousMouseState != isAcceptPressed)
        {
            virtualMouse.CopyState(out MouseState mouseState);
            mouseState.WithButton(MouseButton.Left, isAcceptPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = isAcceptPressed;
        }
    }
    
    void UpdateCursorImage(Vector2 newPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect: canvas.GetComponent<RectTransform>(), 
            screenPoint: newPosition,
            cam: canvas.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, 
            localPoint: out Vector2 anchoredPosition
            );
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    void EnterBuildMode(EnterBuildModeEvent eventInfo)
    {
        cursorTransform.gameObject.SetActive(true);
    }
}
