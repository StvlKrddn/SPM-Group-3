using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : Upgradable
{
    [SerializeField] protected float effectDuration;

    public void UpgradeDuration(float amount, bool isPercentual)
    {
        effectDuration = UpgradeValue(effectDuration, amount, isPercentual);
    }

    public float EffectDuration { get { return effectDuration; } }
}
