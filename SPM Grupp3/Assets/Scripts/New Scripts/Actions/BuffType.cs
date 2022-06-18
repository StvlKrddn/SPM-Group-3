using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffType : ActionType
{
    /// <summary> How many seconds does the buff last </summary>
    [Tooltip("How many seconds does the buff last")]
    [SerializeField] private float buffDuration;

    public virtual void ApplyBuff(GameObject target) { }
}
