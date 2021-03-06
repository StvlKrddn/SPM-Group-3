using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TankUpgradeTree : MonoBehaviour
{
    private FadeBehaviour abilityFill;
    private FadeBehaviour abilityIcon;
    private Slider slider;
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
                slider.value = 0;

                if (abilityFill.Faded())
                    abilityFill.Fade();

                StartCoroutine(RechargeAbilityBar());
            }
        }
    }

    protected virtual void Start()
	{   
        tankState = GetComponent<TankState>();
        weapon = GetComponent<WeaponSlot>();
        gameManager = FindObjectOfType<GameManager>();
        GameObject abilityUi = gameObject.transform.Find("AbilityUI").gameObject;

        if(tankState.gameObject.GetComponent<SniperTank>().isActiveAndEnabled == true)
            abilityIcon = abilityUi.gameObject.transform.Find("SniperIcon").gameObject.GetComponent<FadeBehaviour>();
        else
            abilityIcon = abilityUi.gameObject.transform.Find("DynamiteIcon").gameObject.GetComponent<FadeBehaviour>();

        slider = abilityUi.GetComponent<Slider>();

        abilityFill = abilityUi.transform.Find("Fill").GetComponent<FadeBehaviour>();
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
            StartCoroutine(UseAbilityBar());          
            return true;
        }
        return false;
    }

    private float elapsed = 0f;

    private IEnumerator UseAbilityBar()
    {
        abilityReady = false;

        elapsed = 0f;
        if (!abilityIcon.Faded())
            abilityIcon.Fade();

        if (abilityFill.Faded())
            abilityFill.Fade();

        while (elapsed < abilityDuration)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(slider.value, slider.minValue, elapsed / abilityDuration);
            yield return null;
        }

        StartCoroutine(RechargeAbilityBar());
    }

    private IEnumerator RechargeAbilityBar()
    {
        float coolDown = abilityCD - abilityDuration;

        elapsed = 0f;

        while (elapsed < coolDown)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, elapsed / coolDown);
            yield return null;
        }

        if (abilityIcon.Faded())
            abilityIcon.Fade();

        if (!abilityFill.Faded())
            abilityFill.Fade();

        ResetCooldown();
    }

    public void ResetCooldown()
    {
        abilityReady = true;
    }
}
