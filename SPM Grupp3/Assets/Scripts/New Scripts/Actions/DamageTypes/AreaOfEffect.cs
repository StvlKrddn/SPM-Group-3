using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaOfEffectDamage", menuName = "DamageType/AreaOfEffect", order = 4)]
public class AreaOfEffectDamage : DamageType
{
    [SerializeField] private float hitRadius;
    [SerializeField] private DamageType damageType;

    public void UpgradeHitRadius(float amount, bool isPercentual)
    {
        hitRadius = base.UpgradeValue(hitRadius, amount, isPercentual);
    }

    public override string ToString()
    {
        return nameof(AreaOfEffectDamage);
    }

    public float GetHitRadius { get { return hitRadius; } }
    public DamageType GetDamageType { get { return damageType; } }
}
