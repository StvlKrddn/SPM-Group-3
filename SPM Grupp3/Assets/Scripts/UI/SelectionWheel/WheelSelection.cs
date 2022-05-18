using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/* Based on: https://answers.unity.com/questions/1592985/how-would-i-create-an-item-wheel-using-analog-stic.html*/
public class WheelSelection : MonoBehaviour
{
    public PlayerInput playerInput;

    // Put in order from right going counter-clockwise
    public Image[] MenuItems;

    // Is the first item centered on the X-axis?
    public bool FirstCenteredX = true;

    // Smallest degree that measure the first item. Should be 0 or lower
    private float firstItemDegrees = 0;

    // Current direction the player is pointing, -1 is no input
    private float pointingAngle;
    private float degreesPerItem;
    private float overflowFirstItem;
    private float numberOfMenuItems;
    private int selectedIndex = -1;

    private InputAction stickAction;
    private InputAction selectAction;
    private Vector2 stickInput;

    private void Start() 
    {
        numberOfMenuItems = MenuItems.Length;
        degreesPerItem = 360 / numberOfMenuItems;

        if (FirstCenteredX)
        {
            firstItemDegrees = 0 - degreesPerItem / 2;
            overflowFirstItem = 360 - degreesPerItem / 2;
        }
        else 
        {
            overflowFirstItem = 0;
            firstItemDegrees = 0;
        }

        playerInput.SwitchCurrentActionMap("Builder");
        stickAction = playerInput.actions["LeftStick"];
        selectAction = playerInput.actions["Accept"];
    }

    private void Update() 
    {
        stickInput = stickAction.ReadValue<Vector2>();

        if (stickAction.IsPressed())
        {
            float signedAngle = Vector2.SignedAngle(Vector2.up, stickInput);
            if (signedAngle <= 0)
            {
                signedAngle += 360;
            }

            // Get angle of stick
            pointingAngle = signedAngle;
            
            selectedIndex = GetIndex(pointingAngle);
        }
        else 
        {
            selectedIndex = -1;
        }

        HighlightItem(selectedIndex);

        if (selectAction.IsPressed() && selectedIndex != -1)
        {
            SelectItem(selectedIndex);
        }
    }

    // Returns index of item based on angle
    private int GetIndex(float angle)
    {
        // Check if there are any menu items
        if (numberOfMenuItems <= 0 || angle < 0)
        {
            return -1;
        }

        // Check if pointing at the first item because it can cross axis
        print(overflowFirstItem);
        if (angle < firstItemDegrees + degreesPerItem || angle > overflowFirstItem)
        {
            return 0;
        }

        int itemIndex = -1;
        int counter = 0;

        for (int i = 1; i <= numberOfMenuItems; i++)
        {
            if (angle > i * degreesPerItem + firstItemDegrees && angle < i * (degreesPerItem + degreesPerItem) + firstItemDegrees)
            {
                counter++;
                itemIndex = i;
            }
        }

        print(counter);
        return itemIndex;
    }

    void HighlightItem(int index)
    {
        if (numberOfMenuItems <= 0)
        {
            return;
        }

        for (int i = 0; i < numberOfMenuItems; i++)
        {
            if (i == index)
            {
                // Hover effect
                MenuItems[i].color = Color.green;
            }
            else
            {
                // Remove hover effect from all other items
                MenuItems[i].color = Color.red;
            }
        }
    }

    void SelectItem(int index)
    {
        bool isPressed = selectAction.IsPressed();
        GameObject selectedItem = MenuItems[index].gameObject;
        if (isPressed)
        {
            MenuItems[index].color = Color.blue;
        }
        else
        {
            MenuItems[index].color = Color.green;
        }
    }
}
