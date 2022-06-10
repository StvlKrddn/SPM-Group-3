using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public abstract void TakeDamage<T>() where T : DamageType;
}