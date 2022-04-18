using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class BuilderController : MonoBehaviour
{
    [SerializeField] private Transform garage;
    [SerializeField] private RectTransform cursorTransform;

    private TankController tank;
    private PlayerInput playerInput;

    private Mouse virtualMouse;

    void OnEnable()
    {
        EventHandler.Instance.RegisterListener<GarageEvent>(EnterBuildMode);
        InputSystem.onAfterUpdate += UpdateMotion;

        if (virtualMouse == null)
        {
            virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
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
        EventHandler.Instance.UnregisterListener<GarageEvent>(EnterBuildMode);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    void UpdateMotion()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnterBuildMode(GarageEvent eventInfo)
    {
        print("Entered Build Mode!");
        //player.PlayerInput.SwitchCurrentActionMap("Builder");
    }
}
