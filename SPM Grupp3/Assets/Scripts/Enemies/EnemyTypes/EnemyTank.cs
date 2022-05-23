using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : EnemyController
{
	public float armor = 5;
	public float armorThreshold = 5;
	public override void TakeDamage(float damage)
	{
		if (damage >= armorThreshold)
		{
			damage -= armor;
			if	(damage < armorThreshold)
			{
				damage = armorThreshold;
			}
			base.TakeDamage(damage);
		}
	}
}
