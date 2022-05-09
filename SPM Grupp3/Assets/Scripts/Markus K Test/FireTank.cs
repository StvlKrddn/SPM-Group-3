using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTank : TankUpgradeTree
{
	private int fireRateIncrease = 2;
	[SerializeField] private GameObject grenade;

	protected override void UpgradeOne()
	{
		if (currentUpgrade == 0 && gameManager.SpendResources(upgradeOneMoney, upgradeOneMaterial))
		{
			currentUpgrade++;
			weapon.FireRate *= fireRateIncrease;
		}
	}
	protected override void UpgradeTwo()
	{
		if (currentUpgrade == 1 && gameManager.SpendResources(upgradeTwoMoney, upgradeTwoMaterial))
		{
			currentUpgrade++;
			tankState.movementSpeed += movementSpeedIncrease;
			//Eldkastare
		}
	}

	protected override void UpgradeThree()
	{
		if (currentUpgrade == 2 && gameManager.SpendResources(upgradeThreeMoney, upgradeThreeMaterial))
		{
			currentUpgrade++;
		}
	}

	public override void Ability()
	{
		if (currentUpgrade >= 3)
		{
			abilityReady = false;
			Instantiate(grenade, tankState.gameObject.transform);
			StartCoroutine(ResetAbility());
		}
		//Ability not unlocked
	}

	
}
