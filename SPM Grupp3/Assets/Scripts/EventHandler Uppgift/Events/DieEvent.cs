using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEvent : Event
{
    public readonly GameObject Invoker;
    public readonly AudioClip[] DeathSounds;     // If object has multiple death sounds, for example to play a random one
    public readonly GameObject DeathParticles;


    public DieEvent(string description, GameObject invoker, AudioClip[] deathSounds, GameObject deathParticles) : base(description)
    {
        Invoker = invoker;
        DeathSounds = deathSounds;
        DeathParticles = deathParticles;
    }
}
