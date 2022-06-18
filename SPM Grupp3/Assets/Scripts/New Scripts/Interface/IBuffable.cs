using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> An entity that can be affected by a buff
public interface IBuffable
{
    public Dictionary<Type, BuffType> CurrentBuffs
    {
        get;
        set;
    }

    public abstract void ApplyBuff(List<ActionType> actionTypes);
    public abstract void RemoveBuff(List<ActionType> actionTypes);
}
