using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageType : ActionType
{
    /// <summary> Status effect applied on hit, null if no status effect should be applied </summary>
    [Tooltip("Status effects that are applied on hit, leave empty if no status effect should be applied")]
    [SerializeField] protected StatusEffect[] statusEffects;

}
