using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialFire : EnemyController
{
    private bool iceShieldOn = true;
    [SerializeField] private float iceRestiance = 0.5f;
    [SerializeField] private GameObject iceShield;

	protected override void OnEnable()
	{
		base.OnEnable();
        iceShield.SetActive(true);
        iceShieldOn = true;
    }

	public override void TakeDamage(float damage)
    {
        if (iceShieldOn == true)
        {
            damage *= iceRestiance;
        }
        base.TakeDamage(damage);
    }

	public override void HitByFire(float damage)
	{
        if (iceShieldOn == true)
        {
            iceShield.SetActive(false);
            iceShieldOn = false;
        }
		base.HitByFire(damage);
	}
}
