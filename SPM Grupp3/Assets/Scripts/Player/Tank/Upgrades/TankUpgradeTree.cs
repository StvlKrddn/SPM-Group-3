using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TankUpgradeTree : MonoBehaviour
{
    private GameObject abilityUi;
    private Slider slider;
    private FadeBehaviour fadeBehaviour;
    private float abilityDuration = 0.5f;

    protected GameManager gameManager;
    protected TankState tankState;
    protected WeaponSlot weapon;

    [Header("Upgrade 2: ")]
    [SerializeField] protected int movementSpeed = 12;

    [Header("Individual: ")]
    [SerializeField] protected float abilityCD;
    [SerializeField] protected bool abilityReady = false;



    private void OnEnable()
    {
        if (UpgradeController.currentUpgradeLevel == 3 && abilityReady == false)
        {
            if (slider != null)
            {
                slider.value = 1;
                StartCoroutine(UseAbilityBar());
            }
        }
    }

    protected virtual void Start()
	{   
        tankState = GetComponent<TankState>();
        weapon = GetComponent<WeaponSlot>();
        gameManager = FindObjectOfType<GameManager>();
        abilityUi = gameObject.transform.Find("AbilityUI").gameObject;
        slider = abilityUi.GetComponent<Slider>();
        fadeBehaviour = abilityUi.GetComponent<FadeBehaviour>();
	}

    public virtual void UpgradeOne() {}

    public virtual void UpgradeTwo()
    {
        tankState.IncreaseSpeed(movementSpeed);
    }

    public virtual void UpgradeThree()
    {
        StartCoroutine(UseAbilityBar());
    }

    public virtual bool Ability()
    {
        if (abilityReady == true)
        {
            abilityReady = false;
            StartCoroutine(UseAbilityBar());          
            return true;
        }
        return false;
    }

    private IEnumerator UseAbilityBar()
    {

        float elapsed = 0f;

        if (fadeBehaviour.Faded())
          fadeBehaviour.Fade();

        if (slider.value != 0)
        {
            while (elapsed < abilityDuration)
            {
                elapsed += Time.deltaTime;

                // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
                slider.value = Mathf.Lerp(slider.value, slider.minValue, elapsed / abilityDuration);
                yield return null;
            }
        }

        float coolDown = abilityCD - abilityDuration;

        elapsed = 0f;

        while (elapsed < coolDown)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, elapsed / coolDown);
            yield return null;
        }
        abilityReady = true;
        if (fadeBehaviour.Faded() == false)
            fadeBehaviour.Fade();
    }

    public void ResetCooldown()
    {
        abilityReady = true;
    }
}
