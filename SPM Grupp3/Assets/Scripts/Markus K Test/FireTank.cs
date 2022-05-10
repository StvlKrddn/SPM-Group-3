using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTank : TankUpgradeTree
{
	[SerializeField] private int fireRateIncrease = 2;
	[SerializeField] private GameObject grenade;

	protected override bool UpgradeOne()
	{
		if (base.UpgradeOne())
		{
			weapon.FireRate *= fireRateIncrease;
			return true;
		}
		return false;
	}
	protected override bool UpgradeTwo()
	{
		if (base.UpgradeTwo())
		{
			//Eldkastare
			return true;
		}
		return false;
			
	}

	public override bool Ability()
	{
		if (base.Ability())
		{
			GameObject grenadeTemp = Instantiate(grenade, transform.position, transform.rotation, null);
			grenadeTemp.transform.parent = null;
			return true;
		}
		return false;
	}

}
