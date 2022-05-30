using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTank : TankUpgradeTree
{
	[SerializeField] private int range = 25;
	[SerializeField] private float fireRateFirst = 1.2f;
    [SerializeField] private float fireRateSecond = 0.5f;

	[SerializeField] private int damage = 75;
	[SerializeField] private GameObject sniperAbility;


	public override void UpgradeOne()
	{
		weapon.MakeSniper(range, fireRateFirst, damage);
		weapon.ClearBullets();
		
	}
	public override void UpgradeTwo()
	{   
		base.UpgradeTwo();
        weapon.MakeSniper(range, fireRateSecond, damage);
        weapon.MaxRange();
	}

	public override bool Ability()
	{
		if (base.Ability())
		{
			Instantiate(sniperAbility, weapon.bulletSpawner.position, weapon.bulletSpawner.rotation, transform);	
			return true;
		}
		return false;
	}

}
