using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTank : TankUpgradeTree
{
	private int rangeIncrease = 5;
	private int fireRateMultiply = 2;
	private int damageIncrease = 25;

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
		throw new System.NotImplementedException();
	}

	public override bool Ability()
	{
		throw new System.NotImplementedException();
	}

}
