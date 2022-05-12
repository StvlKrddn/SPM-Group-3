using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryEvent : Event
{
    public int Money;
    public int Material;
    public int EnemiesKilled;
    public int TowersBuilt;

    public VictoryEvent(string description, int money, int material, int enemiesKilled, int towersBuilt) : base(description)
    {
        Money = money;
        Material = material;
        EnemiesKilled = enemiesKilled;
        TowersBuilt = towersBuilt;
    }
}
