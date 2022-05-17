using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : EnemyController
{
	public float armor;
	public float armorThreshold = 5;
	public override void TakeDamage(float damage)
	{
		damage -= armor;
		Debug.Log(damage);
		if (damage < armorThreshold)
		{
			damage = armorThreshold;
		}
		base.TakeDamage(damage);
	}
}
