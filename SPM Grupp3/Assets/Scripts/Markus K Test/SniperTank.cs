using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTank : TankUpgradeTree
{
	[SerializeField] private int rangeIncrease = 5;
	[SerializeField] private int fireRateMultiply = 3;
	[SerializeField] private int damageIncrease = 50;
	[SerializeField] private GameObject sniperAbility;


	public override bool UpgradeOne()
	{
		if (base.UpgradeOne())
		{
			weapon.MakeSniper(rangeIncrease, fireRateMultiply, damageIncrease);
			return true;
		}
		return false;
	}
	public override bool UpgradeTwo()
	{
		if (base.UpgradeTwo())
		{	
			weapon.MaxRange();
			return true;
		}
		return false;
	}

	public override bool Ability()
	{
		if (base.Ability())
		{
			Instantiate(sniperAbility, weapon.bulletSpawner.position, weapon.bulletSpawner.rotation);	
			return true;
		}
		return false;
	}

}
