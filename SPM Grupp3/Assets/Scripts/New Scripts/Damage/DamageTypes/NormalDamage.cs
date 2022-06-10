using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDamage : Upgradable
{
    [SerializeField] private float damage;

    public void UpgradeDamage(float amount, bool isPercentual)
    {
        damage = base.UpgradeValue(damage, amount, isPercentual);
    }

    public float GetDamage { get { return damage; } }
}
