using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(TankState))]
public class TankController : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private float movementSpeed = 6f;

    // Components
    Rigidbody rb;
    Transform turretObject;

    // Input components
    InputAction moveGamepadAction;
    InputAction aimAction;

    // Instance variables
    Vector2 gamepadInputVector;
    protected Vector3 aimInputVector;
    float aimSpeed;
    float standardSpeed;
    Matrix4x4 isoMatrix;

    // Getters and Setters
    public float StandardSpeed { 
        get 
        { 
            // Shady code until i figure out a fix
            if (standardSpeed == 0)
            {
                standardSpeed = movementSpeed;
            }
            return standardSpeed; 
        } 
        set { standardSpeed = value; } }

    void Start()
    {
        InitializeInputSystem();

        standardSpeed = movementSpeed;

        rb = GetComponent<Rigidbody>();

        turretObject = transform.GetChild(0);
        
        aimSpeed = standardSpeed * 5;

        //Create isometric matrix
        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        /* Eplanation of isometric translation can be found here: https://youtu.be/8ZxVBCvJDWk */
    }

    
    
    void InitializeInputSystem()
    {
        PlayerInput playerInput = GetComponent<TankState>().PlayerInput;

        moveGamepadAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
    }

    void Update()
    {
        gamepadInputVector = moveGamepadAction.ReadValue<Vector2>();
        aimInputVector = aimAction.ReadValue<Vector2>();

        RotateTurret(); 
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Translate the input vector to the right plane
        Vector3 movementVector = new Vector3(gamepadInputVector.x, 0, gamepadInputVector.y);

        // Translate vector to an isometric viewpoint
        Vector3 skewedVector = TranslateToIsometric(movementVector);
        
        Vector3 movement = skewedVector * standardSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);

        if (moveGamepadAction.IsPressed())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(skewedVector), Time.deltaTime * standardSpeed);
        }
    }

    void RotateTurret()
    {
        Vector3 aimVector = new Vector3(aimInputVector.x, 0, aimInputVector.y);

        Vector3 skewedVector = TranslateToIsometric(aimVector);

        if (aimAction.IsPressed())
        {
            turretObject.rotation = Quaternion.Slerp(turretObject.rotation, Quaternion.LookRotation(skewedVector), Time.deltaTime * aimSpeed);
        }
    } 

    Vector3 TranslateToIsometric(Vector3 vector)
    {
        // Skewer the input vector 45 degrees to accomodate for the isometric perspective
        return isoMatrix.MultiplyPoint3x4(vector);
    }
}