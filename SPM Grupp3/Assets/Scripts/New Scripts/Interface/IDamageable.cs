using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float Health
    {
        get;
        set;
    }

    public abstract void TakeHit(DamageType damageType);
}
