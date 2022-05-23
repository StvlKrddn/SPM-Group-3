using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndEvent : Event
{
    public int CurrentWave;

    public WaveEndEvent(string description, int currentWave) : base(description)
    {
        CurrentWave = currentWave;
    }
}
