using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectDamage", menuName = "DamageType/StatusEffectDamage", order = 3)]
public class StatusEffectDamage : DamageType
{
    [SerializeField] private StatusEffect statusEffect;

    public override string ToString()
    {
        return nameof(StatusEffectDamage);
    }
}
