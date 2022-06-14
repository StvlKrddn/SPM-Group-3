using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireRateBuff", menuName = "BuffType/FireRateBuff", order = 2)]
public class FireRateBuff : BuffType
{
    /// <summary> Flat increase of fire rate for the buff duration </summary>
    [Tooltip("Flat increase of fire rate during the buff duration")]
    [SerializeField] protected float fireRateIncrease;

    public void UpgradeDamageIncrease(float amount, bool isPercentual)
    {
        fireRateIncrease = base.UpgradeValue(fireRateIncrease, amount, isPercentual);
    }
}
