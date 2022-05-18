using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TankPointer : MonoBehaviour
{
    private Matrix4x4 isometricMatrix;
    private TankState player;
    private PlayerInput playerInput;
    private InputAction aimAction;
    private Vector2 aimInputVector;
    private RectTransform pointer;
    private float aimSpeed;


    private void Awake() 
    {
        isometricMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        player = transform.parent.GetComponentInParent<TankState>();
        playerInput = player.PlayerInput;
        aimAction = playerInput.actions["Aim"];
        aimSpeed = player.StandardSpeed * 5;
        pointer = GetComponent<RectTransform>();
    }

    private void Update() 
    {
        aimInputVector = aimAction.ReadValue<Vector2>();
        RotatePointer();
    }

    void RotatePointer()
    {
        Vector3 aimVector = new Vector3(aimInputVector.x, 0, aimInputVector.y);

        Vector3 skewedVector = TranslateToIsometric(aimVector);

        if (aimAction.IsPressed())
        {
            pointer.rotation = Quaternion.Slerp(pointer.rotation, Quaternion.LookRotation(skewedVector), Time.deltaTime * aimSpeed);
        }
    }

    Vector3 TranslateToIsometric(Vector3 vector)
    {
        // Skewer the input vector 45 degrees to accommodate for the isometric perspective
        return isometricMatrix.MultiplyPoint3x4(vector);
    }
}
