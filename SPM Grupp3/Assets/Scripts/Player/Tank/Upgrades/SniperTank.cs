using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTank : TankUpgradeTree
{
	[Space]
	[SerializeField] private int range = 25;
	[SerializeField] private float level1FireRate = 1.2f;
	[SerializeField] private int level1Damage = 75;
	[Space]
	[SerializeField] private float level2FireRate = 0.5f;
	[SerializeField] private int level2Penetration;
	[SerializeField] private int level2Damage = 75;
	[Space]
	[SerializeField] private GameObject level1Mesh;
	[SerializeField] private GameObject level2Mesh;
	[SerializeField] private GameObject sniperAbility;


	public override void UpgradeOne()
	{
		weapon.MakeSniper(range, level1FireRate, level1Damage);
		weapon.ClearBullets();
		weapon.ChangeTurretMesh(level1Mesh);
		
	}
	public override void UpgradeTwo()
	{   
		base.UpgradeTwo();
		weapon.UpgradeFirerate(level2FireRate);
		weapon.UpgradeDamage(level2Damage);
        weapon.MaxRange(level2Penetration);
		weapon.ChangeTurretMesh(level2Mesh);
	}

	public override void UpgradeThree()
	{
		base.UpgradeThree();
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
