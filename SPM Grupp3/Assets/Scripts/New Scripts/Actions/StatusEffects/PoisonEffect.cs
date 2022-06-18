using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEffect", menuName = "StatusEffect/PoisonEffect", order = 1)]
public class PoisonEffect : StatusEffect
{
    [SerializeField] private float tickDamage;
    [SerializeField] private float tickFrequency;

    private float effectTimer;
    private float tickTimer;
    private DamageHandler target;

    public float GetTickDamage { get { return tickDamage; } }
    public float GetFrequency { get { return tickFrequency; } }

    public void UpgradeTickDamage(float amount, bool isPercentual)
    {
        tickDamage = base.UpgradeValue(tickDamage, amount, isPercentual);
    }

    public void UpgradeTickFrequency(float amount, bool isPercentual)
    {
        tickFrequency = base.UpgradeValue(tickFrequency, amount, isPercentual);
    }

    public override void Applied(GameObject target)
    {
        effectTimer = 0f;
        tickTimer = 0f;
        this.target = target.GetComponent<DamageHandler>();
    }

    public override void UpdateEffect()
    {
        float tickInterval = 1 / tickFrequency;
        if (effectTimer < effectDuration)
        {
            effectTimer += Time.deltaTime;
            tickTimer += Time.deltaTime;
            if (tickTimer > tickInterval)
            {
                Tick();
                tickTimer = 0f;
            }
        }
        else
        {
            target.RemoveStatusEffect(this);
        }
    }

    public override void Tick()
    {
        if (target != null)
        {
            target.TakeDamage(tickDamage);
        }
    }
}
