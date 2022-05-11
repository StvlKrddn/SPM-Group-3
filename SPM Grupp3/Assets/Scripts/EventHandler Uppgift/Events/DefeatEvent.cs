using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEvent : Event
{
    public int Wave;
    public GameObject KilledBy;
    public int EnemiesKilled;

    public DefeatEvent(string description, int wave, GameObject killedBy, int enemiesKilled) : base(description)
    {
        Wave = wave;
        KilledBy = killedBy;
        EnemiesKilled = enemiesKilled;
    }
}
