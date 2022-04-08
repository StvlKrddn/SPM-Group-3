using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    // NOTE(August): OM DEN RÖR PÅ SIG KONSTIGT KAN DET BERO PÅ ATT ROTATIONEN ÄR LÅST PÅ RIGIDBODY

    [Header("Movement properties")]
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float turnSpeed = 2;

    private Rigidbody rb;
    private PlayerInput playerInput;

    // Caching input actions
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction shootAction;

    private Vector3 inputVector;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        InitializeInputSystem();
    }

    void InitializeInputSystem()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
        shootAction = playerInput.actions["Shoot"];
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
    }

    void FixedUpdate()
    {
        Turn();
        Move();
    }

    void GatherInput()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), 0);
    }

    void Move()
    {
        Vector3 movement = transform.forward * inputVector.x * movementSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    void Turn()
    {
        Vector3 rotationVector = new Vector3(0, inputVector.y * turnSpeed * Time.deltaTime * 100f, 0);
        Quaternion rotation = Quaternion.Euler(rotationVector);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
