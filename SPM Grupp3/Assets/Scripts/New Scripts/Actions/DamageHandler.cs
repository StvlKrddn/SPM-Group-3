using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageHandler : MonoBehaviour, IDamageable
{
    private LayerMask targetLayerMask;
    [SerializeField] private float health = 100;
    [SerializeField] List<StatusEffect> currentStatusEffects;
    public float Health { get => health; set => health = value; }

    public void TakeHit(List<ActionType> actionTypes)
    {
        foreach (DamageType damageType in actionTypes)
        {
            damageType.DealDamage(this);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currentStatusEffects.Clear();
            Destroy(gameObject);
        }
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (!currentStatusEffects.Contains(statusEffect))
        {
            currentStatusEffects.Add(statusEffect);
            statusEffect.Applied(gameObject);
        }
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        currentStatusEffects.Remove(statusEffect);
        statusEffect.Removed();
    }

    private void Update()
    {
        foreach (StatusEffect effect in currentStatusEffects.ToList())
        {
            effect.UpdateEffect();
        }
    }
}
