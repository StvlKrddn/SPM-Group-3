using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgradable : MonoBehaviour
{
    /// <summary> Upgrade a value in a damage type </summary>
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
}
