using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTank : TankUpgradeTree
{
	private int fireRateDivide = 2;
	private int damageIncrease = 25;

	protected override bool UpgradeOne()
	{
		if (base.UpgradeOne())
		{
			weapon.FireRate /= fireRateDivide;
			weapon.UpgradeDamage(damageIncrease);
			return true;
		}
		return false;
	}
	protected override bool UpgradeTwo()
	{
		throw new System.NotImplementedException();
	}

	protected override bool UpgradeThree()
	{
		throw new System.NotImplementedException();
	}

	public override bool Ability()
	{
		throw new System.NotImplementedException();
	}

}
