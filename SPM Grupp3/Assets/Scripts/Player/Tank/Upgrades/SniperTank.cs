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
	[SerializeField] private GameObject level1Mesh;
	[SerializeField] private GameObject level3Mesh;


	public override void UpgradeOne()
	{
		weapon.MakeSniper(range, fireRateFirst, damage);
		weapon.ClearBullets();
		weapon.ChangeTurretMesh(level1Mesh);
		
	}
	public override void UpgradeTwo()
	{   
		base.UpgradeTwo();
        weapon.MakeSniper(range, fireRateSecond, damage);
        weapon.MaxRange();
	}

	public override void UpgradeThree()
	{
		base.UpgradeThree();
		weapon.ChangeTurretMesh(level3Mesh);
	}

	public override bool Ability()
	{
		if (base.Ability())
		{
			Instantiate(sniperAbility, weapon.BulletSpawner.position, weapon.BulletSpawner.rotation, transform);	
			return true;
		}
		return false;
	}

}
