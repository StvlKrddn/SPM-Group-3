using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    // NOTE(August): OM DEN RÖR PÅ SIG KONSTIGT KAN DET BERO PÅ ATT ROTATIONEN ÄR LÅST PÅ RIGIDBODY

    [Header("Movement properties")]
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float turnSpeed = 2;

    private float movementInput;
    private float rotationInput;

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        Turn();
        Move();
    }

    void GatherInput()
    {
        // For moving back and forth
        movementInput = Input.GetAxisRaw("Vertical");

        // For rotating tank
        rotationInput = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        transform.Translate(0, 0, movementInput * movementSpeed * Time.deltaTime);
    }

    void Turn()
    {
        transform.Rotate(0, rotationInput * turnSpeed, 0);
    }
}
