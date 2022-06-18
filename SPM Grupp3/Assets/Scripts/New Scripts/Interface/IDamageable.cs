using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> An entity that can take damage </summary>
public interface IDamageable
{
    float Health
    {
        get;
        set;
    }

    public abstract void TakeHit(List<ActionType> actionTypes);

    public abstract void TakeDamage(float damage);

    public abstract void ApplyStatusEffect(StatusEffect statusEffect);

    public abstract void RemoveStatusEffect(StatusEffect statusEffect);
}
