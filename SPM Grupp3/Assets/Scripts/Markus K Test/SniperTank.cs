using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTank : TankUpgradeTree
{
	[SerializeField] private int range = 25;
	[SerializeField] private float fireRate = 1.2f;
	[SerializeField] private int damage = 75;
	[SerializeField] private GameObject sniperAbility;


	public override void UpgradeOne()
	{
		weapon.MakeSniper(range, fireRate, damage);
		
	}
	public override void UpgradeTwo()
	{
		base.UpgradeTwo();
		weapon.MaxRange();
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
