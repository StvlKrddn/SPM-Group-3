using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(TankState))]
public class BoostAbility : MonoBehaviour
{
    private Slider slider;
    private InputAction boostAction;
    private TankState tankState;
    private bool allowedToBoost = true;
    private float speedBeforeBoost;
    private float boostTimer;
    private float notInUseTimer;

    // Inspector variables
    [SerializeField] private float boostAccelerationTimeMultiplier = 8f;
    [SerializeField] private float boostSpeedMultiplier = 5f;
    [SerializeField] private float boostDuration = 1f;
    [SerializeField] private float boostCooldownTime = 5f;
    [SerializeField] private GameObject boosters;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject boostUI;
    private FadeBehaviour fadeBehaviour;
    
    // Getters and setters
    public float BoostSpeedMultiplier { get { return boostSpeedMultiplier; } set { boostSpeedMultiplier = value; } }
    public float BoostDuration { get { return boostDuration; } set { boostDuration = value; } }
    public float BoostCooldownTime { get { return boostCooldownTime; } set { boostCooldownTime = value; } }

    void Awake()
    {
        tankState = GetComponent<TankState>();
        slider = boostUI.GetComponent<Slider>();
        slider.value = 1f;
        boostAction = tankState.PlayerInput.actions["Boost"];
        fadeBehaviour = boostUI.GetComponent<FadeBehaviour>();
        ChangeSpeed();
    }

    void Update()
    {
        Boost();
    }

    private void OnEnable() 
    {
        if (allowedToBoost == false)
        {
            slider.value = 0;
            StartCoroutine(BoostCooldown());
        }
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

        if(notInUseTimer > BoostCooldownTime && !fadeBehaviour.Faded())
        {
            fadeBehaviour.Fade();
        }
    }

    private IEnumerator UseBoostBar()
    {
        float boostDuration = BoostDuration;

        float elapsed = 0f;

        if(fadeBehaviour.Faded() == true)
            fadeBehaviour.Fade();

        if (slider.value != 0)
        { 
            while (elapsed < boostDuration)
            {
                elapsed += Time.deltaTime;

                // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
                slider.value = Mathf.Lerp(1f, 0, elapsed / boostDuration);
                yield return null;
            }
        }

        slider.value = 0;

        float coolDown = BoostCooldownTime - BoostDuration;

        elapsed = 0f;

        while (elapsed < coolDown)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(0, 1f, elapsed / coolDown);
            yield return null;
        }

        if (fadeBehaviour.Faded() == false)
            fadeBehaviour.Fade();
        slider.value = 1f;
    }

    IEnumerator BoostCooldown()
    {
        allowedToBoost = false;
        StartCoroutine(UseBoostBar());
        yield return new WaitForSeconds(boostCooldownTime);
        allowedToBoost = true;
    }
}
