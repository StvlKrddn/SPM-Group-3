using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankUpgradeTree : MonoBehaviour
{
    protected GameManager gameManager;
    protected TankState tankState;
    protected WeaponSlot weapon;
    [SerializeField] protected int currentUpgrade = 0;


    [Header("Upgrade 1: ")]
    [SerializeField] protected int upgradeOneMoney;
    [SerializeField] protected int upgradeOneMaterial;

    [Header("Upgrade 2: ")]
    [SerializeField] protected int upgradeTwoMoney;
    [SerializeField] protected int upgradeTwoMaterial;
    [SerializeField] protected int movementSpeedIncrease = 25;

    [Header("Upgrade 3: ")]
    [SerializeField] protected int upgradeThreeMoney;
    [SerializeField] protected int upgradeThreeMaterial;

    [Header("Individual: ")]
    [SerializeField] protected float abilityCD;
    [SerializeField] protected bool abilityReady;

    protected virtual void Start()
	{
		tankState = GetComponent<TankState>();
        weapon = GetComponent<WeaponSlot>();
        gameManager = FindObjectOfType<GameManager>(); 
	}

    public virtual bool UpgradeOne()
    {
        if (currentUpgrade == 0 && gameManager.SpendResources(upgradeOneMoney, upgradeOneMaterial))
        {
            currentUpgrade = 1;
            return true;
        }
        return false;
    }

    public virtual bool UpgradeTwo()
    {
        if (currentUpgrade == 1 && gameManager.SpendResources(upgradeTwoMoney, upgradeTwoMaterial))
        {
            currentUpgrade = 2;
            tankState.IncreaseSpeed(movementSpeedIncrease);
            return true;
        }
        return false;
    }

    public virtual bool UpgradeThree()
    {
        if (currentUpgrade == 2 && gameManager.SpendResources(upgradeThreeMoney, upgradeThreeMaterial))
        {
            currentUpgrade = 3;
            abilityReady = true;
            return true;
        }
        return false;
    }

    public virtual bool Ability()
    {
        if (currentUpgrade == 3 && abilityReady == true)
        {
            Debug.Log("Kastar cool spell");
            abilityReady = false;
            StartCoroutine(ResetAbility());
            return true;
        }
        return false;
    }

    protected IEnumerator ResetAbility()
    {
        yield return new WaitForSeconds(abilityCD);
		Debug.Log("We back");
        abilityReady = true;
    }

}
