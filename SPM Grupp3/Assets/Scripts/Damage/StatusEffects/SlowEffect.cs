using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : StatusEffect
{
    [SerializeField] private float slowAmount;

    public void UpgradeSlowAmount(float amount, bool isPercentual)
    {
        slowAmount = base.UpgradeValue(slowAmount, amount, isPercentual);
    }

    public float GetSlowAmount { get { return slowAmount; } }
}
