using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    [SerializeField] protected float effectDuration;

    /// <summary> Upgrade a value in an action type </summary>
    /// <param name="baseValue"> The value to be upgraded </param>
    /// <param name="increaseValue"> How much the baseValue should increase </param>
    /// <param name="isPercentual"> 
    ///     Is it a flat or percentual increase?
    ///     If false, increaseValue is added to baseValue.
    ///     If true, increaseValue is multiplied by baseValue 
    /// </param>
    /// <returns> The upgraded value </returns>
    protected float UpgradeValue(float baseValue, float increaseValue, bool isPercentual)
    {
        if (isPercentual)
        {
            baseValue *= increaseValue;
        }
        else
        {
            baseValue += increaseValue;
        }
        return baseValue;
    }

    public abstract void Applied(GameObject target);
    public virtual void UpdateEffect() { }
    public virtual void Tick() { }
    public virtual void Removed() { }

    public float EffectDuration { get { return effectDuration; } }
}
