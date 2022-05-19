using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankUpgradeTree : MonoBehaviour
{
    protected GameManager gameManager;
    protected TankState tankState;
    protected WeaponSlot weapon;

    [Header("Upgrade 2: ")]
    [SerializeField] protected int movementSpeed = 12;

    [Header("Individual: ")]
    [SerializeField] protected float abilityCD;
    [SerializeField] protected bool abilityReady = false;

    private UpgradeController uC;

    protected virtual void Start()
	{   
        uC = UpgradeController.Instance;
        tankState = GetComponent<TankState>();
        weapon = GetComponent<WeaponSlot>();
        gameManager = FindObjectOfType<GameManager>(); 
	}

    public abstract void UpgradeOne();

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
   //         StartCoroutine(ResetAbility());
            return true;
        }
        return false;
    }

    protected IEnumerator ResetAbility()
    {
        yield return new WaitForSeconds(abilityCD);
        abilityReady = true;
    }

    public void ResetColdown()
    {
        abilityReady = true;
    }

}
