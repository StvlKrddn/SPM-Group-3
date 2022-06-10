using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageDealer
{
    public abstract void HitTarget<D>(GameObject target) where D : DamageType;

    public abstract void AddDamageType<D>() where D : DamageType;
}
