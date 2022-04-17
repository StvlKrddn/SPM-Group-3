using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEvent : Event
{
    public GameObject Invoker;
    public AudioClip[] DeathSounds;     // If object has multiple death sounds, for example to play a random one
    public GameObject DeathParticles;


    public DieEvent(string description, GameObject invoker, AudioClip[] deathSounds, GameObject deathParticles) : base(description)
    {
        Invoker = invoker;
        DeathSounds = deathSounds;
        DeathParticles = deathParticles;
    }
}
