using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : EnemyController
{
	public int armor;

	public override void TakeDamage(float damage)
	{
		damage -= armor;
		base.TakeDamage(damage);
	}
}
