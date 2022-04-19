using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankController))]
public class BoostAbility : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private float boostSpeedMultiplier = 5f;
    [SerializeField] private float boostDuration = 1f;
    [SerializeField] private float boostCooldownTime = 5f;

    InputAction boostAction;
    TankController tank;

    float speedBeforeBoost;
    bool allowedToBoost = true;
    float boostTimer;
    float boostAccelerationTimeMultiplier = 8f;

    // Getters and setters
    public float BoostSpeedMultiplier { get { return boostSpeedMultiplier; } set { boostSpeedMultiplier = value; } }
    public float BoostDuration { get { return boostDuration; } set { boostDuration = value; } }
    public float BoostCooldownTime { get { return boostCooldownTime; } set { boostCooldownTime = value; } }

    void Start()
    {
        tank = GetComponent<TankController>();

        speedBeforeBoost = tank.StandardSpeed;
        boostAction = tank.PlayerInput.actions["Boost"];
    }

    void Update()
    {
        Boost();
    }

    void Boost()
    {
        if (boostAction.IsPressed() && allowedToBoost)
        {
            boostTimer = boostDuration;
            StartCoroutine(BoostCooldown());
        }

        // If the boost timer is not yet finished
        if (boostTimer > 0f)
        {   
            // Subtract elapsed time since last frame from timer
            boostTimer -= Time.deltaTime;

            // Multiply movement speed
            tank.StandardSpeed = Mathf.Lerp(tank.StandardSpeed, speedBeforeBoost * boostSpeedMultiplier, Time.deltaTime * boostAccelerationTimeMultiplier);
        }
        else
        {
            // Reset movement speed
            tank.StandardSpeed = Mathf.Lerp(tank.StandardSpeed, speedBeforeBoost, Time.deltaTime * boostAccelerationTimeMultiplier);
        }
    }

    IEnumerator BoostCooldown()
    {
        allowedToBoost = false;
        yield return new WaitForSeconds(boostCooldownTime);
        allowedToBoost = true;
    }
}
