using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(TankState))]
public class BoostAbility : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private float boostSpeedMultiplier = 5f;
    [SerializeField] private float boostDuration = 1f;
    [SerializeField] private float boostCooldownTime = 5f;
    [SerializeField] private GameObject boosters;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject boostUI;
    
    private Slider slider;

    InputAction boostAction;
    TankState tankState;
    GameObject effect;

    float speedBeforeBoost;
    bool allowedToBoost = true;
    float boostTimer;
    float notInUseTimer;
    float boostAccelerationTimeMultiplier = 8f;

    // Getters and setters
    public float BoostSpeedMultiplier { get { return boostSpeedMultiplier; } set { boostSpeedMultiplier = value; } }
    public float BoostDuration { get { return boostDuration; } set { boostDuration = value; } }
    public float BoostCooldownTime { get { return boostCooldownTime; } set { boostCooldownTime = value; } }

    void Awake()
    {
        tankState = GetComponent<TankState>();
        slider = boostUI.GetComponent<Slider>();
        slider.value = 0.5f;
        boostAction = tankState.PlayerInput.actions["Boost"];
        ChangeSpeed();
    }

    void Update()
    {
        Boost();
    }

    private void OnEnable() 
    {
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
            // Play particle effect
            foreach (Transform child in boosters.transform)
            {
                child.GetComponent<ParticleSystem>().Play();
            }

            notInUseTimer = 0;

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

            

            animator.SetBool("isBoosting", true);
        }
        else
        {
            // Reset movement speed
            tankState.StandardSpeed = Mathf.Lerp(tankState.StandardSpeed, speedBeforeBoost, Time.deltaTime * boostAccelerationTimeMultiplier);
            foreach (Transform child in boosters.transform)
            {
                child.GetComponent<ParticleSystem>().Play();
            }

            notInUseTimer += Time.deltaTime;
            animator.SetBool("isBoosting", false);
        }

        if(notInUseTimer > BoostCooldownTime && !boostUI.GetComponent<FadeBehaviour>().Faded())
        {
            boostUI.GetComponent<FadeBehaviour>().Fade();
        }
    }

    private IEnumerator UseBoostBar()
    {
        float boostDuration = BoostDuration;

        float elapsed = 0f;

        if(boostUI.GetComponent<FadeBehaviour>().Faded())
            boostUI.GetComponent<FadeBehaviour>().Fade();

        while (elapsed < boostDuration)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(0.5f, 0, elapsed / boostDuration);
            yield return null;
        }

        slider.value = 0;

        float coolDown = BoostCooldownTime - BoostDuration;

        elapsed = 0f;

        while (elapsed < coolDown)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(0, 0.5f, elapsed / coolDown);
            yield return null;
        }

        slider.value = 0.5f;
    }

    IEnumerator BoostCooldown()
    {
        allowedToBoost = false;
        StartCoroutine(UseBoostBar());
        yield return new WaitForSeconds(boostCooldownTime);
        allowedToBoost = true;
    }
}
