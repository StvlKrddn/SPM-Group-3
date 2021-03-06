using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTank : TankUpgradeTree
{
	[Space]
	[SerializeField] private float fireRate;
	[Space]
	public GameObject level1Mesh;
	[SerializeField] private GameObject level2Mesh;
	[SerializeField] private GameObject grenade;

	public override void UpgradeOne()
	{
		weapon.UpgradeFirerate(fireRate);
		weapon.ChangeTurretMesh(level1Mesh);
	}
	public override void UpgradeTwo()
	{
		base.UpgradeTwo();
		weapon.ClearBullets();
		weapon.ChangeTurretMesh(level2Mesh);
		weapon.enabled = false;
		weapon.transform.Find("FireLVL2").GetComponent<Outline>().enabled = false;
	}

	public override bool Ability()
	{
		if (base.Ability())
		{	
			GameObject grenadeTemp = Instantiate(grenade, transform.position, grenade.transform.rotation, transform);
			grenadeTemp.transform.parent = null;
			return true;
		}
		return false;
	}

}
