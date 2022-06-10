using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalDamage", menuName = "DamageTypes/NormalDamage", order = 1)]
public class NormalDamage : DamageType
{
    [SerializeField] private float damage;

    public void UpgradeDamage(float amount, bool isPercentual)
    {
        damage = base.UpgradeValue(damage, amount, isPercentual);
    }

    public float GetDamage { get { return damage; } }
}
