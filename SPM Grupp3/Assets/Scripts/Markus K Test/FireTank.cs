using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTank : TankUpgradeTree
{
	[SerializeField] private int fireRateMultiply = 2;
	[SerializeField] private Transform flameThrower;
	[SerializeField] private GameObject grenade;

	protected override void Start()
	{
		base.Start();
		// grenade = tempgrenade 
	}

	public override bool UpgradeOne()
	{
		if (base.UpgradeOne())
		{
			weapon.UpgradeFirerate(fireRateMultiply);
			return true;
		}
		return false;
	}
	public override bool UpgradeTwo()
	{
		if (base.UpgradeTwo())
		{
			weapon.enabled = false;
			flameThrower.gameObject.SetActive(true);
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
