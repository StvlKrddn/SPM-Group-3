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
    [SerializeField] private Animator boostUIAnimator;
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
        slider.maxValue = 1f;
        slider.minValue = 0f;
        boostAction = tankState.PlayerInput.actions["Boost"];
        fadeBehaviour = boostUI.GetComponent<FadeBehaviour>();
        ChangeSpeed();
    }

    void Update()
    {
        Boost();
        boostUIAnimator.SetFloat("Stamina", slider.value);
    }

    private void OnEnable() 
    {
        if (allowedToBoost == false)
        {
            slider.value = 0;
            StartCoroutine(RechargeBoostBar());
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

    private float elapsed = 0f;

    private IEnumerator UseBoostBar()
    {

        if(fadeBehaviour.Faded() == true)
            fadeBehaviour.Fade();

        elapsed = 0f;

        while (elapsed < BoostDuration)
        {
             elapsed += Time.deltaTime;
            
            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(slider.maxValue, slider.minValue, elapsed / BoostDuration);
             yield return null;      
        }
        slider.value = slider.minValue;

        StartCoroutine(RechargeBoostBar());
    }

    private IEnumerator RechargeBoostBar()
    { 
        float coolDown = BoostCooldownTime - BoostDuration;
        notInUseTimer = 0;

        elapsed = 0f;

        while (elapsed < coolDown)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, elapsed / coolDown);
            yield return null;
        }

/*        if (fadeBehaviour.Faded() == false)
            fadeBehaviour.Fade();
*/
        slider.value = slider.maxValue;

        allowedToBoost = true;
    }

    IEnumerator BoostCooldown()
    {
        allowedToBoost = false;
        StartCoroutine(UseBoostBar());
        yield return new WaitForSeconds(boostCooldownTime);
        //allowedToBoost = true;
    }
}
