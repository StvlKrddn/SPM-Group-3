using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageEvent : Event
{
    public GameObject Entity;
    public float Damage;
    public float Ptc;

    public TakeDamageEvent(string description, GameObject entity, float damage, float ptc) : base(description)
    {
        Entity = entity;
        Damage = damage;
        Ptc = ptc;
    }
}
