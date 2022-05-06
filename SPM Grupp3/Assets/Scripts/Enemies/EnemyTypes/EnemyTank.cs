using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : EnemyController
{
	public float armor;
	/*
	public override void TakeDamage(float damage)
	{
		damage -= armor;
		Debug.Log(damage);
		if (damage > 0)
		{
			base.TakeDamage(damage);
		}
	}
	*/
}
