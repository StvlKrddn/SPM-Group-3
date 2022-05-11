using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankState))]
public class BoostAbility : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private float boostSpeedMultiplier = 5f;
    [SerializeField] private float boostDuration = 1f;
    [SerializeField] private float boostCooldownTime = 5f;

    InputAction boostAction;
    TankState tankState;

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
        tankState = GetComponent<TankState>();
        boostAction = tankState.PlayerInput.actions["Boost"];
        ChangeSpeed();
    }

    void Update()
    {
        Boost();
    }

    private void OnEnable() 
    {
        // Skapar ett exploit där spelaren kan gå in och ut ur Garaget för att få tillbaka sin boost direkt men who cares :)
        allowedToBoost = true;
    }

    public void ChangeSpeed()
    {
        speedBeforeBoost = tankState.StandardSpeed;
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
            tankState.StandardSpeed = Mathf.Lerp(tankState.StandardSpeed, speedBeforeBoost * boostSpeedMultiplier, Time.deltaTime * boostAccelerationTimeMultiplier);
        }
        else
        {
            // Reset movement speed
            tankState.StandardSpeed = Mathf.Lerp(tankState.StandardSpeed, speedBeforeBoost, Time.deltaTime * boostAccelerationTimeMultiplier);
        }
    }

    IEnumerator BoostCooldown()
    {
        allowedToBoost = false;
        yield return new WaitForSeconds(boostCooldownTime);
        allowedToBoost = true;
    }
}
