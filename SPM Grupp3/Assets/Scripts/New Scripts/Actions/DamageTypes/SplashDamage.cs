using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SplashDamage", menuName = "DamageType/SplashDamage", order = 2)]
public class SplashDamage : DamageType
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

    public override string ToString()
    {
        return nameof(SplashDamage);
    }

    public override void DealDamage(DamageHandler target)
    {
        Collider[] targetsInRange = Physics.OverlapSphere(
            position: target.transform.position,
            radius: splashRadius,
            layerMask: affectedLayer
        );

        foreach (Collider collider in targetsInRange)
        {
            if (collider.GetComponent<DamageHandler>())
            {
                DamageHandler targetInRange = collider.GetComponent<DamageHandler>();
                ApplyStatusEffect(targetInRange);
                targetInRange.TakeDamage(splashDamage);
            }
        }
    }

    public float GetSplashDamage { get { return splashDamage; } }
    public float GetSplashRadius { get { return splashRadius; } }
}
