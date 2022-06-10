using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : StatusEffect
{
    [SerializeField] private float tickDamage;
    [SerializeField] private float tickFrequency;

    public void UpgradeTickDamage(float amount, bool isPercentual)
    {
        tickDamage = base.UpgradeValue(tickDamage, amount, isPercentual);
    }

    public void UpgradeTickFrequency(float amount, bool isPercentual)
    {
        tickFrequency = base.UpgradeValue(tickFrequency, amount, isPercentual);
    }

    public float GetTickDamage { get { return tickDamage; } }
    public float GetFrequency { get { return tickFrequency; } }
}
