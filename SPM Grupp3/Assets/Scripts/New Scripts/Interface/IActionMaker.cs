using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> An entity that can perform an action </summary>
public interface IActionPerformer
{

    public List<ActionType> ActionTypes { get; }

    // public abstract void AddActionType<A>() where A : ActionType;
    // public abstract void RemoveActionType<A>() where A : ActionType;

    public abstract void HitTarget(IDamageable target);
    public abstract void ApplyBuff(IBuffable target);
}
