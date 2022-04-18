using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWaveEvent : Event
{
    public int CurrentWave;

    public NewWaveEvent(string description, int currentWave) : base(description)
    {
        CurrentWave = currentWave;
    }
}
