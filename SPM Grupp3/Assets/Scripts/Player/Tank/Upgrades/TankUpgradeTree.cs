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
            StartCoroutine(ResetAbility());
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
        abilityReady = true;
    }

    public virtual bool Ability()
    {
        if (abilityReady == true)
        {
            abilityReady = false;
            StartCoroutine(UseAbilityBar());
            StartCoroutine(ResetAbility());
            return true;
        }
        return false;
    }

    protected IEnumerator ResetAbility()
    {
        yield return new WaitForSeconds(abilityCD);
        abilityReady = true;
    }

    private IEnumerator UseAbilityBar()
    {

        float elapsed = 0f;

        if (fadeBehaviour.Faded())
          fadeBehaviour.Fade();

        while (elapsed < abilityDuration)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(1f, 0, elapsed / abilityDuration);
            yield return null;
        }

        slider.value = 0f;

        float coolDown = abilityCD - abilityDuration;

        elapsed = 0f;

        while (elapsed < coolDown)
        {
            elapsed += Time.deltaTime;

            // preChangePct is start value and the goal is pct. elapsed / updateSpeedSeconds is the equation per activation
            slider.value = Mathf.Lerp(0, 1f, elapsed / coolDown);
            yield return null;
        }
        fadeBehaviour.Fade();
        slider.value = 1f;
    }

    public void ResetCooldown()
    {
        abilityReady = true;
    }
}
