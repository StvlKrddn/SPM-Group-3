using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectDamage : Upgradable
{
    [SerializeField] private float hitRadius;
    [SerializeField] private DamageType damageType;

    public void UpgradeHitRadius(float amount, bool isPercentual)
    {
        hitRadius = base.UpgradeValue(hitRadius, amount, isPercentual);
    }

    public float HitRadius { get { return hitRadius; } }
}
