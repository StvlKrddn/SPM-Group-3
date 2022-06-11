using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageDealer
{
    Dictionary<string, DamageType> DamageTypes
    {
        get;
    }

    public abstract void AddDamageType<D>() where D : DamageType;

    public abstract void HitTarget(IDamageable target);
}
