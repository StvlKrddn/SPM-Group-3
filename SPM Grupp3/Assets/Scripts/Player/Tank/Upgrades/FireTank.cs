using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTank : TankUpgradeTree
{
	[SerializeField] private float fireRate = 0.2f;
	[SerializeField] private Transform flameThrower;
	[SerializeField] private GameObject grenade;


	public override void UpgradeOne()
	{
		weapon.UpgradeFirerate(fireRate);
	}
	public override void UpgradeTwo()
	{
		base.UpgradeTwo();
		weapon.ClearBullets();
		weapon.enabled = false;
		flameThrower.gameObject.SetActive(true);
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
