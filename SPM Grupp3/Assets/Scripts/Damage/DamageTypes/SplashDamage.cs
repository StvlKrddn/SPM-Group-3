using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : Upgradable
{
    [SerializeField] private float splashDamage;
    [SerializeField] private float splashRadius;

    public void UpgradeSplashDamage(float amount, bool isPercentual)
    {
        splashDamage = base.UpgradeValue(splashDamage, amount, isPercentual);
    }

    public void UpgradeSplashRadius(float amount, bool isPercentual)
    {
        splashRadius = base.UpgradeValue(splashRadius, amount, isPercentual);
    }

    public float GetSplashDamage { get { return splashDamage; } }
    public float GetSplashRadius { get { return splashRadius; } }
}
