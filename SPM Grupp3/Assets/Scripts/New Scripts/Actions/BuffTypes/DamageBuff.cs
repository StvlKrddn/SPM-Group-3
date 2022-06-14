using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBuff", menuName = "BuffType/DamageBuff", order = 1)]
public class DamageBuff : BuffType
{
    /// <summary> Flat increase of damage for the buff duration </summary>
    [Tooltip("Flat increase of damage during the buff duration")]
    [SerializeField] protected float damageIncrease;

    public void UpgradeDamageIncrease(float amount, bool isPercentual)
    {
        damageIncrease = base.UpgradeValue(damageIncrease, amount, isPercentual);
    }
}
