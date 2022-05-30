using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEvent : Event
{
    public AudioClip sound;

    public PlaySoundEvent (string description, AudioClip sound) : base(description)
    {
        this.sound = sound;
    }
}
